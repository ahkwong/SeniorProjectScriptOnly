using Assets.Scripts.Entities;
using Senior.Globals;
using UnityEditor;

namespace Senior.Editorr
{
    [CustomEditor(typeof(Entity), true)]
    public class EntityEditor : Editor
    {
        public SerializedProperty currentFaction;
        public SerializedProperty alliedFactions;
        public SerializedProperty enemyFactions;

        void OnEnable()
        {
            currentFaction = serializedObject.FindProperty("currentFaction");
            alliedFactions = serializedObject.FindProperty("alliedFactions");
            enemyFactions = serializedObject.FindProperty("enemyFactions");

        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            serializedObject.Update();

            currentFaction.intValue = (int)((Faction)EditorGUILayout.EnumMaskField("Current Faction", (Faction)currentFaction.intValue));
            alliedFactions.intValue = (int)((Faction)EditorGUILayout.EnumMaskField("Allied Factions", (Faction)alliedFactions.intValue));
            enemyFactions.intValue = (int)((Faction)EditorGUILayout.EnumMaskField("Enemy Factions", (Faction)enemyFactions.intValue));

            serializedObject.ApplyModifiedProperties();
        }
    }
}