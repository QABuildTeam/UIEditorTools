using System.Threading.Tasks;

namespace ACFW
{
    public interface IContextController
    {
        /// <summary>
        /// The controller is not open yet
        /// </summary>
        /// <returns></returns>
        Task PreOpen();
        /// <summary>
        /// Open the controller
        /// </summary>
        /// <returns></returns>
        Task Open();
        /// <summary>
        /// The controller has just been opened
        /// </summary>
        /// <returns></returns>
        Task PostOpen();
        /// <summary>
        /// The controller is about to close
        /// </summary>
        /// <returns></returns>
        Task PreClose();
        /// <summary>
        /// Close the controller
        /// </summary>
        /// <returns></returns>
        Task Close();
        /// <summary>
        /// The controller has just been closed
        /// </summary>
        /// <returns></returns>
        Task PostClose();
        bool IsOpen();
    }

    public interface IContextController<T> : IContextController
    {
        T Value { get; set; }
    }
}
