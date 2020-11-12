//=========== Copyright (c) GameBuilders, All rights reserved. ================//

using UnityEditor;
using UnityEngine;

namespace FPSBuilder.Editor
{
    /// <summary>
    /// Common styles used for Post-processing editor controls.
    /// </summary>
    public static class Styling
    {
        public static readonly GUIStyle leftButton;
        public static readonly GUIStyle midButton;
        public static readonly GUIStyle rightButton;

        public static readonly GUIStyle button;
        public static readonly GUIStyle miniButton;

        private static readonly Texture2D paneOptionsIconDark;
        private static readonly Texture2D paneOptionsIconLight;

        /// <summary>
        /// Option icon used in effect headers.
        /// </summary>
        public static Texture2D paneOptionsIcon => EditorGUIUtility.isProSkin ? paneOptionsIconDark : paneOptionsIconLight;

        /// <summary>
        /// Style for header labels.
        /// </summary>
        public static readonly GUIStyle headerLabel;

        /// <summary>
        /// Style for the override checkbox.
        /// </summary>
        public static readonly GUIStyle smallTickbox;

        private static readonly Color headerBackgroundDark;
        private static readonly Color headerBackgroundLight;

        /// <summary>
        /// Color of effect header backgrounds.
        /// </summary>
        public static Color headerBackground => EditorGUIUtility.isProSkin ? headerBackgroundDark : headerBackgroundLight;

        private static readonly Color splitterDark;
        private static readonly Color splitterLight;

        /// <summary>
        /// Color of UI splitters.
        /// </summary>
        public static Color splitter => EditorGUIUtility.isProSkin ? splitterDark : splitterLight;

        public static readonly GUIStyle headerFoldout;
        public static readonly GUIStyle background;

        static Styling()
        {
            headerBackgroundDark = new Color(0.1f, 0.1f, 0.1f, 0.2f);
            headerBackgroundLight = new Color(1f, 1f, 1f, 0.2f);

            splitterDark = new Color(0.12f, 0.12f, 0.12f, 1.333f);
            splitterLight = new Color(0.6f, 0.6f, 0.6f, 1.333f);

            paneOptionsIconDark = (Texture2D)EditorGUIUtility.Load("Builtin Skins/DarkSkin/Images/pane options.png");
            paneOptionsIconLight = (Texture2D)EditorGUIUtility.Load("Builtin Skins/LightSkin/Images/pane options.png");

            headerLabel = new GUIStyle("Label")
            {
                fontSize = 18,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleLeft
            };

            headerFoldout = new GUIStyle("Foldout");
            smallTickbox = new GUIStyle("Toggle");
            background = new GUIStyle("box");

            miniButton = new GUIStyle("minibutton");
            leftButton = new GUIStyle("ButtonLeft");
            midButton = new GUIStyle("ButtonMid");
            rightButton = new GUIStyle("ButtonRight");

            button = new GUIStyle("button")
            {
                fixedHeight = 18
            };
        }
    }
}
