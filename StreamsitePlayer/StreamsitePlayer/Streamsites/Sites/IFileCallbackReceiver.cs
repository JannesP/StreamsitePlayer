using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamsitePlayer.Streamsites.Sites
{
    interface IFileCallbackReceiver
    {

        /// <summary>
        /// Used to display some kind of notification to the user.
        /// </summary>
        /// <param name="remainingTime">remaining time in ms</param>
        /// <param name="max">maximum or starting time in ms</param>
        void FileRequestStatusUpdate(int remainingTime, int max, int rqeuestId);

        void ReceiveFileLink(string file, int requestId);

    }
}
