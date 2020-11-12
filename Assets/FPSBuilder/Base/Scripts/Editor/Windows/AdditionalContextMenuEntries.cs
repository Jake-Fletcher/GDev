using FPSBuilder.Core.Animation;
using FPSBuilder.Core.Managers;
using FPSBuilder.Core.Player;
using FPSBuilder.Core.Weapons;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FPSBuilder.Editor
{
    public static class AdditionalContextMenuEntries
    {
        [MenuItem("GameObject/First-Person Character", false, 20)]
        private static void CreateCharacter()
        {
            EditorGUI.BeginChangeCheck();
            
            GameObject obj_fpsCharacter = new GameObject("FPCharacter");
            FirstPersonCharacterController fpsCharacter = obj_fpsCharacter.AddComponent<FirstPersonCharacterController>();
            HealthController healthController = obj_fpsCharacter.AddComponent<HealthController>();
            healthController.RegisterBodyPart(obj_fpsCharacter.AddComponent<DamageHandler>());
            
            Rigidbody rigidbody = fpsCharacter.GetComponent<Rigidbody>();
            rigidbody.drag = 5;
            rigidbody.useGravity = false;
            rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            CapsuleCollider capsuleCollider = fpsCharacter.GetComponent<CapsuleCollider>();
            capsuleCollider.height = 2;
            
            obj_fpsCharacter.transform.position = new Vector3(0, 1, 0);
            
            GameObject managers = new GameObject("Managers");
            
            GameObject obj_fpsCamera = new GameObject("FPSCamera");
            obj_fpsCamera.AddComponent<AudioListener>();
            
            Camera fpsCamera = obj_fpsCamera.AddComponent<Camera>();

            SerializedObject editor_fpsCharacter = new SerializedObject(fpsCharacter);
            SerializedProperty editor_fpsCharacter_mainCamera = editor_fpsCharacter.FindProperty("m_MainCamera");
            editor_fpsCharacter_mainCamera.objectReferenceValue = fpsCamera;
            editor_fpsCharacter.ApplyModifiedProperties();
            
            // Character Structure
            managers.transform.SetParent(obj_fpsCharacter.transform);
            managers.transform.localPosition = new Vector3(0, 0.5f, 0);

            GameObject fp = new GameObject("FirstPerson");
            fp.transform.SetParent(obj_fpsCharacter.transform);
            fp.transform.localPosition = new Vector3(0, 0, 0);

            GameObject obj_cameraAnimator = new GameObject("CameraAnimator");
            obj_cameraAnimator.transform.SetParent(fp.transform);
            obj_cameraAnimator.transform.localPosition = new Vector3(0, 0, 0);
            CameraAnimator cameraAnimator = obj_cameraAnimator.AddComponent<CameraAnimator>();
            
            SerializedObject editor_cameraAnimator = new SerializedObject(cameraAnimator);
            SerializedProperty editor_cameraAnimator_fpsCharacter = editor_cameraAnimator.FindProperty("m_FPController");
            editor_cameraAnimator_fpsCharacter.objectReferenceValue = fpsCharacter;
            
            SerializedProperty editor_cameraAnimator_healthController = editor_cameraAnimator.FindProperty("m_HealthController");
            editor_cameraAnimator_healthController.objectReferenceValue = healthController;
            
            SerializedProperty editor_cameraAnimator_MotionAnimation = editor_cameraAnimator.FindProperty("m_MotionAnimation");
            editor_cameraAnimator_MotionAnimation.FindPropertyRelative("m_TargetTransform").objectReferenceValue = obj_cameraAnimator.transform;
            editor_cameraAnimator_MotionAnimation.FindPropertyRelative("m_JumpAnimation").FindPropertyRelative("m_TargetPosition").vector3Value = new Vector3(0, 0, 0);
            editor_cameraAnimator_MotionAnimation.FindPropertyRelative("m_JumpAnimation").FindPropertyRelative("m_TargetRotation").vector3Value = new Vector3(-5, 0, 0);
            editor_cameraAnimator_MotionAnimation.FindPropertyRelative("m_JumpAnimation").FindPropertyRelative("m_Duration").floatValue = 0.25f;
            editor_cameraAnimator_MotionAnimation.FindPropertyRelative("m_JumpAnimation").FindPropertyRelative("m_ReturnDuration").floatValue = 0.25f;
            
            editor_cameraAnimator_MotionAnimation.FindPropertyRelative("m_LandingAnimation").FindPropertyRelative("m_TargetPosition").vector3Value = new Vector3(0, -0.1f, 0);
            editor_cameraAnimator_MotionAnimation.FindPropertyRelative("m_LandingAnimation").FindPropertyRelative("m_TargetRotation").vector3Value = new Vector3(5, 0, 0);
            editor_cameraAnimator_MotionAnimation.FindPropertyRelative("m_LandingAnimation").FindPropertyRelative("m_Duration").floatValue = 0.2f;
            editor_cameraAnimator_MotionAnimation.FindPropertyRelative("m_LandingAnimation").FindPropertyRelative("m_ReturnDuration").floatValue = 0.25f;

            editor_cameraAnimator.ApplyModifiedProperties();
            
            GameObject characterBody = new GameObject("CharacterBody");
            characterBody.transform.SetParent(fp.transform);
            characterBody.transform.localPosition = new Vector3(0, 1, 0);
            
            obj_fpsCamera.transform.SetParent(cameraAnimator.transform);
            obj_fpsCamera.transform.localPosition = new Vector3(0, 0.8f, 0);

            GameObject weaponsHandler = new GameObject("WeaponsHandler");
            weaponsHandler.transform.SetParent(obj_fpsCamera.transform);
            weaponsHandler.transform.localPosition = new Vector3(0, 1, 0);
            
            GameObject itemsHandler = new GameObject("ItemsHandler");
            itemsHandler.transform.SetParent(obj_fpsCamera.transform);
            itemsHandler.transform.localPosition = new Vector3(0, 1, 0);

            Selection.activeObject = obj_fpsCharacter;
            
            if (EditorGUI.EndChangeCheck())
            {
                EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            }
        }
        
        [MenuItem("GameObject/Game Controller", false, 21)]
        private static void CreateGameController()
        {
            EditorGUI.BeginChangeCheck();
            
            GameObject gameController = new GameObject("Game Controller");
            gameController.AddComponent<GameplayManager>();
            gameController.AddComponent<AudioManager>();
            gameController.AddComponent<BulletDecalsManager>();
            gameController.tag = "GameController";
            
            Selection.activeObject = gameController;
            
            if (EditorGUI.EndChangeCheck())
            {
                EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            }
        }
    }
}