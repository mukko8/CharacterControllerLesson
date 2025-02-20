using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharMove : MonoBehaviour
{
    Animator animator;
    CharacterController cc;
    AudioSource uniVoice;

    Vector3 dir=Vector3.zero;
    public float gravity=20.0f;
    public float speed=4.0f;
    public float rotSpeed=300.0f;
    public float jumpPower=8.0f;
    void Start()
    {
        animator=GetComponent<Animator>();
        cc=GetComponent<CharacterController>();
        uniVoice=GetComponent<AudioSource>();
    }
    void Update()
    {
        //前進成分を取得(0~1),今回バックはしない
        float acc=Mathf.Max(Input.GetAxis("Vertical"),0f);
        //接地していたら
        if(cc.isGrounded){
            //左右キーで回転
            float rot=Input.GetAxis("Horizontal");
            //前進、回転が入力されていた場合大きいほうの値をspeedにセットする(回転のみをするときも動くモーションをする)
            animator.SetFloat("speed",Mathf.Max(acc,Mathf.Abs(rot)));
            //回転は直接トランスフォームをいじる
            transform.Rotate(0,rot*rotSpeed*Time.deltaTime,0);
            if(Input.GetButtonDown("Jump")){
                //ジャンプモーション開始
                animator.SetTrigger("jump");
            }
        }
        //下方向の重力成分
        dir.y-=gravity*Time.deltaTime;

        //CharacterControllerはMoveでキャラを移動させる
        cc.Move((transform.forward*acc*speed+dir)*Time.deltaTime);
        //移動した後着地していたらy成分を0にする
        if(cc.isGrounded){
            dir.y=0;
        }
    }
    //ジャンプモーションで地面から足が離れたときに呼ばれるイベント
    public void OnJumpStart(){
        //足が離れたらトランスフォームを上方に移動する
        dir.y=jumpPower;
        uniVoice.Play();
    }
}
