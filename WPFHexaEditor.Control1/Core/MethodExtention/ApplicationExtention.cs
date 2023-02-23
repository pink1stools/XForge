//////////////////////////////////////////////
// Apache 2.0  - 2016-2017
// Author : Derek Tremblay (derektremblay666@gmail.com)
//////////////////////////////////////////////

using System.Windows;
using System.Windows.Threading;

namespace WPFHexaEditor.Core.MethodExtention
{
    /// <summary>
    /// DoEvents when control is in long task. Control do not freeze the dispatcher.
    /// </summary>
    public static class ApplicationExtention
    {
        private static DispatcherOperationCallback exitFrameCallback = new DispatcherOperationCallback(ExitFrame);

        public static void DoEvents(this Application app, DispatcherPriority priority = DispatcherPriority.Background)
        {
            DispatcherFrame nestedFrame = new DispatcherFrame();
            DispatcherOperation exitOperation = Dispatcher.CurrentDispatcher.BeginInvoke(priority, exitFrameCallback, nestedFrame);

            try
            {
                //execute all next message
                Dispatcher.PushFrame(nestedFrame);

                //If not completed, will stop it
                if (exitOperation.Status != DispatcherOperationStatus.Completed)
                    exitOperation.Abort();
            }
            catch
            {
                exitOperation.Abort();
            }
        }

        private static object ExitFrame(object f)
        {
            ((DispatcherFrame)f).Continue = false;
            return null;
        }
    }
}