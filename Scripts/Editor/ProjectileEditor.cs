using Senior.Globals;
using Seniors.Skills.Projectiles;
using UnityEditor;

namespace Senior.Editorr
{
    [CustomEditor(typeof(Projectile), true)]

    public class ProjectileEditor : Editor
    {
        public SerializedProperty targetFaction;

        void OnEnable()
        {
            targetFaction = serializedObject.FindProperty("targetFaction");


        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            serializedObject.Update();

            targetFaction.intValue = (int)((Faction)EditorGUILayout.EnumMaskField("Target Faction", (Faction)targetFaction.intValue));

            serializedObject.ApplyModifiedProperties();
        }
    }
}