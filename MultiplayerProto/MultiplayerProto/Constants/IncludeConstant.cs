using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiplayerProto.Constants
{
    using System.Collections;

    using Microsoft.Xna.Framework;

    public class IncludeConstant
    {
        public static String serverip = "192.168.0.14";

        public static int resolutionWidth = 800;
        public static int resolutionHeigth = 600;

        public static int NumberOfCards = 5;
        public static int InitCardsInHand = 4;

        public static int pixel = 32;
        public static int FrameWidth = pixel * 6;
        public static int FrameHeight = pixel * 8;
        public static int leftMargin = 32;

        public static int localCardPosition_Y = 350;
        public static int playerCardPosition_Y = 100;

        public static int localCard1_X = 0;
        public static int localCard2_X = 1 * (FrameWidth + pixel);
        public static int localCard3_X = 2 * (FrameWidth + pixel);
        public static int localCard4_X = 3 * (FrameWidth + pixel);

        public static int[] localCardPositions = new int[] {0, 
                                                            1 * (FrameWidth + pixel),
                                                            2 * (FrameWidth + pixel),
                                                            3 * (FrameWidth + pixel),
                                                            };

        public static Rectangle boundingRect_card1 = new Rectangle(localCardPositions[0], localCardPosition_Y, FrameWidth, FrameHeight);
        public static Rectangle boundingRect_card2 = new Rectangle(localCardPositions[1], localCardPosition_Y, FrameWidth, FrameHeight);
        public static Rectangle boundingRect_card3 = new Rectangle(localCardPositions[2], localCardPosition_Y, FrameWidth, FrameHeight);
        public static Rectangle boundingRect_card4 = new Rectangle(localCardPositions[3], localCardPosition_Y, FrameWidth, FrameHeight);
    }
}
