using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace libfvadWrapper
{
    public class Libfvad : IDisposable
    {
        unsafe struct Fvad { };
        unsafe Fvad* fvad;

        class LibfvadNative
        {
            [DllImport("libfvad.dll", CallingConvention = CallingConvention.Cdecl)]
            internal extern static unsafe Fvad* fvad_new();

            [DllImport("libfvad.dll", CallingConvention = CallingConvention.Cdecl)]
            internal extern static unsafe void fvad_free(Fvad* inst);

            [DllImport("libfvad.dll", CallingConvention = CallingConvention.Cdecl)]
            internal extern static unsafe void fvad_reset(Fvad* inst);

            [DllImport("libfvad.dll", CallingConvention = CallingConvention.Cdecl)]
            internal extern static unsafe int fvad_set_mode(Fvad* inst, int mode);

            [DllImport("libfvad.dll", CallingConvention = CallingConvention.Cdecl)]
            internal extern static unsafe int fvad_set_sample_rate(Fvad* inst, int sample_rate);

            [DllImport("libfvad.dll", CallingConvention = CallingConvention.Cdecl)]
            internal extern static unsafe int fvad_process(Fvad* inst, Int16[] frame, int length);
        }

        public unsafe Libfvad()
        {
            fvad = LibfvadNative.fvad_new();
        }

        /// <summary>
        /// Changes the VAD operating ("aggressiveness") mode of a VAD instance.  Default is "quality"
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public unsafe bool SetMode(FvadAgroMode mode)
        {
            int result = LibfvadNative.fvad_set_mode(fvad, (int)mode);
            return result == 0;
        }

        /// <summary>
        /// resets state and sets mode and sample rate to default
        /// </summary>
        public unsafe void Reset()
        {
            LibfvadNative.fvad_reset(fvad);
        }

        /// <summary>
        /// Sets the input sample rate in Hz for a VAD instance.
        /// <para>
        /// Valid values are 8000, 16000, 32000 and 48000. The default is 8000.
        /// </para>
        /// </summary>
        /// <param name="sample_rate"></param>
        /// <returns></returns>
        public unsafe bool SetSampleRate(int sample_rate)
        {
            return LibfvadNative.fvad_set_sample_rate(fvad, sample_rate) == 0;
        }

        /// <summary>
        /// Calculates a VAD decision for an audio frame.
        /// 
        /// </summary>
        /// <param name="frame">array of 16-bit samples</param>
        /// <param name="length">must be 10, 20 or 30 ms, so for 8khz, lenght should be 80, 160, or 240</param>
        /// <returns></returns>
        public unsafe bool HasActiveVoice(Int16[] frame, int length)
        {
            int result = LibfvadNative.fvad_process(fvad, frame, length);
            if (result == -1)
                throw new ArgumentException("invalid frame length");

            return result == 1;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected unsafe virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    LibfvadNative.fvad_free(fvad);
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        ~Libfvad()
        {
          // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
          Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }

    public enum FvadAgroMode
    {
        quality,
        low_bitrate,
        aggressive,
        very_aggressive
    }
}
