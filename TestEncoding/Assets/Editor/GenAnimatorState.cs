using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditorInternal;

public class GenAnimatorState : Editor
{
    [MenuItem("Plugin/CreateAnimatorState")]
    static void DoCreateAnimationAssets()
    {
        CreateAnimatorState("gailun");
        CreateAnimatorState("jiuweiyaohu");
    }

    public static void CreateAnimatorState(string strHero)
    {
        AnimatorController animatorController = AnimatorController.CreateAnimatorControllerAtPath("Assets/Resources/AnimatorController/" + strHero + ".controller");
        //得到它的Layer， 默认layer为base 你可以去拓展
        AnimatorControllerLayer layer = animatorController.GetLayer(0);

        AddOneState("idle", "Assets/Artworks/Model/" + strHero + "/fbx/" + strHero + "_001_01@h01.FBX", layer);
        AddOneState("run", "Assets/Artworks/Model/" + strHero + "/fbx/" + strHero + "_001_01@m01.FBX", layer);
        AddOneState("a", "Assets/Artworks/Model/" + strHero + "/fbx/" + strHero + "_001_01@a01.FBX", layer);
        AddOneState("a1", "Assets/Artworks/Model/" + strHero + "/fbx/" + strHero + "_001_01@a01.FBX", layer);
        AddOneState("a2", "Assets/Artworks/Model/" + strHero + "/fbx/" + strHero + "_001_01@a02.FBX", layer);
        AddOneState("d", "Assets/Artworks/Model/" + strHero + "/fbx/" + strHero + "_001_01@d01.FBX", layer);
        AddOneState("q", "Assets/Artworks/Model/" + strHero + "/fbx/" + strHero + "_001_01@q01.FBX", layer);
        AddOneState("w", "Assets/Artworks/Model/" + strHero + "/fbx/" + strHero + "_001_01@w01.FBX", layer);
        AddOneState("e", "Assets/Artworks/Model/" + strHero + "/fbx/" + strHero + "_001_01@e01.FBX", layer);
        AddOneState("r", "Assets/Artworks/Model/" + strHero + "/fbx/" + strHero + "_001_01@r01.FBX", layer);

        AddIdleState("Assets/Artworks/Model/" + strHero + "/fbx/" + strHero + "_001_01@h01.FBX", layer);
        AddMoveState("Assets/Artworks/Model/" + strHero + "/fbx/" + strHero + "_001_01@m01.FBX", layer);
        AddSeekState("Assets/Artworks/Model/" + strHero + "/fbx/" + strHero + "_001_01@m01.FBX", layer);
        AddHitState("Assets/Artworks/Model/" + strHero + "/fbx/" + strHero + "_001_01@d01.FBX", layer);
        AddAttackState("Assets/Artworks/Model/" + strHero + "/fbx/" + strHero + "_001_01@a01.FBX", layer);
        AddAllOtherState("Assets/Artworks/Model/" + strHero + "/fbx/" + strHero + "_001_01@h01.FBX", layer);

        Debug.Log("Export " +strHero + "OK!");
    }

    private static void AddStateTransition(string path, AnimatorControllerLayer layer)
    {
        UnityEditorInternal.StateMachine sm = layer.stateMachine;
        //根据动画文件读取它的AnimationClip对象
        AnimationClip newClip = AssetDatabase.LoadAssetAtPath(path, typeof(AnimationClip)) as AnimationClip;
        //取出动画名子 添加到state里面
        State state = sm.AddState(newClip.name);
        state.SetAnimationClip(newClip, layer);
        //把state添加在layer里面
        Transition trans = sm.AddAnyStateTransition(state);
        //把默认的时间条件删除
        trans.RemoveCondition(0);
    }

    private static void AddIdleState(string path, AnimatorControllerLayer layer)
    {
        UnityEditorInternal.StateMachine sm = layer.stateMachine;

        //根据动画文件读取它的AnimationClip对象
        AnimationClip newClip = AssetDatabase.LoadAssetAtPath(path, typeof(AnimationClip)) as AnimationClip;
        //取出动画名子 添加到state里面
		State state = sm.AddState("BaseIdle");
		state.SetAnimationClip(newClip, layer);
		state = sm.AddState("BaseFightIdle");
		state.SetAnimationClip(newClip, layer);
		state = sm.AddState("MotionIdle");
		state.SetAnimationClip(newClip, layer);
	}

	private static void AddMoveState(string path, AnimatorControllerLayer layer)
	{
		UnityEditorInternal.StateMachine sm = layer.stateMachine;
		
		//根据动画文件读取它的AnimationClip对象
		AnimationClip newClip = AssetDatabase.LoadAssetAtPath(path, typeof(AnimationClip)) as AnimationClip;
		//取出动画名子 添加到state里面
		State state = sm.AddState("MoveWalk");
		state.SetAnimationClip(newClip, layer);
		state = sm.AddState("MoveRun");
		state.SetAnimationClip(newClip, layer);
	}

	private static void AddSeekState(string path, AnimatorControllerLayer layer)
	{
		UnityEditorInternal.StateMachine sm = layer.stateMachine;
		
		//根据动画文件读取它的AnimationClip对象
		AnimationClip newClip = AssetDatabase.LoadAssetAtPath(path, typeof(AnimationClip)) as AnimationClip;
		//取出动画名子 添加到state里面
		State state = sm.AddState("SeekWalk");
		state.SetAnimationClip(newClip, layer);
		state = sm.AddState("SeekRun");
		state.SetAnimationClip(newClip, layer);
	}
	private static void AddHitState(string path, AnimatorControllerLayer layer)
	{
		UnityEditorInternal.StateMachine sm = layer.stateMachine;
		
		//根据动画文件读取它的AnimationClip对象
		AnimationClip newClip = AssetDatabase.LoadAssetAtPath(path, typeof(AnimationClip)) as AnimationClip;
		//取出动画名子 添加到state里面
		State state = sm.AddState("HitHit");
		state.SetAnimationClip(newClip, layer);
		state = sm.AddState("HitHitLeft");
		state.SetAnimationClip(newClip, layer);
		state = sm.AddState("HitHitRight");
		state.SetAnimationClip(newClip, layer);
		state = sm.AddState("HitKnock");
		state.SetAnimationClip(newClip, layer);
		state = sm.AddState("KnockHeavy");
		state.SetAnimationClip(newClip, layer);
		state = sm.AddState("HitDie");
		state.SetAnimationClip(newClip, layer);
	}

	private static void AddAttackState(string path, AnimatorControllerLayer layer)
	{
		UnityEditorInternal.StateMachine sm = layer.stateMachine;
		
		//根据动画文件读取它的AnimationClip对象
		AnimationClip newClip = AssetDatabase.LoadAssetAtPath(path, typeof(AnimationClip)) as AnimationClip;
		//取出动画名子 添加到state里面
		State state = sm.AddState("Attack1");
		state.SetAnimationClip(newClip, layer);
		state = sm.AddState("Attack2");
		state.SetAnimationClip(newClip, layer);
		state = sm.AddState("Attack3");
		state.SetAnimationClip(newClip, layer);
		state = sm.AddState("Attack4");
		state.SetAnimationClip(newClip, layer);
	}

	private static void AddOneState(string statename, string path, AnimatorControllerLayer layer)
	{
		UnityEditorInternal.StateMachine sm = layer.stateMachine;
		
		//根据动画文件读取它的AnimationClip对象
		AnimationClip newClip = AssetDatabase.LoadAssetAtPath(path, typeof(AnimationClip)) as AnimationClip;
        if (newClip != null)
        {
            //取出动画名子 添加到state里面
            State state = sm.AddState(statename);
            state.SetAnimationClip(newClip, layer);
        }
	}

	private static void AddAllOtherState(string path, AnimatorControllerLayer layer)
	{
		UnityEditorInternal.StateMachine sm = layer.stateMachine;
		
		//根据动画文件读取它的AnimationClip对象
		AnimationClip newClip = AssetDatabase.LoadAssetAtPath(path, typeof(AnimationClip)) as AnimationClip;
		//取出动画名子 添加到state里面
		State state = sm.AddState("HitRevive");
		state.SetAnimationClip(newClip, layer);

		state = sm.AddState("HitRise");
		state.SetAnimationClip(newClip, layer);
		state = sm.AddState("HitRise2");
		state.SetAnimationClip(newClip, layer);
		state = sm.AddState("MotionShow01");
		state.SetAnimationClip(newClip, layer);
		state = sm.AddState("MotionAppear");
		state.SetAnimationClip(newClip, layer);

		for (int i = 1; i <= 10; i++)
		{
			state = sm.AddState("BaseRideIdle" + i.ToString("00"));
			state.SetAnimationClip(newClip, layer);

			state = sm.AddState("MoveRideRun" + i.ToString("00"));
			state.SetAnimationClip(newClip, layer);
		}
		
	}



}