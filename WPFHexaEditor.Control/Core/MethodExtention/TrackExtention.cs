//////////////////////////////////////////////
// Apache 2.0  - 2016-2017
// Author : Derek Tremblay (derektremblay666@gmail.com)
//////////////////////////////////////////////

using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace WPFHexaEditor.Core.MethodExtention
{
    public static class TrackExtention
    {
        /// <summary>
        /// Get actual top position of track
        /// </summary>
        public static double Top(this Track track)
        {
            Grid parent = track.Parent as Grid;
            RepeatButton TopRepeatButton = (RepeatButton)parent.Children[1];

            return TopRepeatButton.ActualHeight + parent.Margin.Top + 1;
        }

        /// <summary>
        /// Get actual bottom position of track
        /// </summary>
        public static double Bottom(this Track track)
        {
            Grid parent = track.Parent as Grid;
            Track TrackControl = (Track)parent.Children[2];

            return TrackControl.Top() +
                TrackControl.ActualHeight +
                parent.Margin.Top + 1;
        }

        /// <summary>
        /// Get actual bottom position of track
        /// </summary>
        public static double ButtonHeight(this Track track) => track.Top() - 1;

        /// <summary>
        /// Get actual Tick Height
        /// </summary>
        public static double TickHeight(this Track track) => track.ActualHeight / track.Maximum;

        /// <summary>
        /// Get actual Tick Height with another maximum value
        /// </summary>
        public static double TickHeight(this Track track, long maximum) => track.ActualHeight / maximum;
    }
}