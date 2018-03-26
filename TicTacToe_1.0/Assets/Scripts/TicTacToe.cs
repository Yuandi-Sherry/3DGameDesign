using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TicTacToe : MonoBehaviour {
    //initialize some constants
	private int ButtonLength = Screen.width/15, LabelWidth = 200, LabelHeight = 100;
	private bool inGameProcess = false, inGameScene = false;
	private int count = 0, Owin = 0, Xwin = 0;
    private string tempResult = "";//to record the result of the round
    public Texture2D player0, player1, player2,singleGrid;
    
	private bool initPlayer = true, Player = true;//true 1st player, false 2nd player
	private int  [,] state = new int[3,3];
	void start() {
		for(int i = 0; i < 3; i++) {
            for(int j = 0; j < 3; j++) {
                state[i, j] = 0;
            }
        }
	}
    void reset() {
        inGameProcess = true;
        initPlayer = !initPlayer;
        Player = initPlayer;
        tempResult = "";
        count = 0;
        for(int i = 0; i < 3; i++) {
            for(int j = 0; j < 3; j++) {
                state[i, j] = 0;
            }
        }
    }
    void OnGUI() {
        GUIStyle buttonStyle = new GUIStyle();  
        buttonStyle.normal.background = singleGrid;    //设置背景填充  
        buttonStyle.normal.textColor = Color.white;
        buttonStyle.fontSize = Screen.width/30;
        buttonStyle.alignment = TextAnchor.MiddleCenter;
        GUI.backgroundColor = new Color(1, 1, 1,1);
    	if(!inGameScene) {
    		if(GUI.Button(new Rect((Screen.width - ButtonLength*2)/2, (Screen.height - ButtonLength*2)/3, ButtonLength*2, ButtonLength),"START",buttonStyle)) {
    			inGameScene = true;
                inGameProcess = true;
    		}
    		GUIStyle fontStyle0 = new GUIStyle();  
	        fontStyle0.alignment = TextAnchor.UpperCenter;  
	        fontStyle0.normal.background = null;    //设置背景填充  
	        fontStyle0.normal.textColor= new Color(120F/256,190F/256,197F/256);   //设置字体颜色  
	        fontStyle0.fontSize = Screen.width/48;       //字体大小  
    		GUI.Label(new Rect((Screen.width - ButtonLength*2)/2, (Screen.height - ButtonLength*2)/2+100,ButtonLength*2, 300),
    			"Tic-tac-toe (also known as noughts and crosses or Xs and Os)\nis paper-and-pencil game for two players, X and O, who take\nturns marking the spaces in a 3×3 grid. The player who succeeds\nin placing three of their marks in a horizontal, vertical, or\ndiagonal row wins the game.",fontStyle0);
    	}
    	if(inGameScene) {
    		for(int i = 0; i < 3; i++) {
	    		for(int j = 0; j < 3; j++) {
	    			if (GUI.Button(new Rect((Screen.width - ButtonLength)/2 + (j-1)*ButtonLength, (Screen.height - ButtonLength)/2 + (i-1)*ButtonLength, ButtonLength, ButtonLength), 
                        state[i,j] == 0 ? player0: (state[i,j] == 1 ? player1 : player2),buttonStyle)) {
	    				if(state[i,j] == 0 && inGameProcess) {
	    					count++;
                            print(count);
		            		if(Player) {
		            			state[i,j] = 1;
		            		}
		            		else {
		            			state[i,j] = 2;
		            		}
		            		Player = !Player;
	    				}
	    			}
	    		}
	    	}
            if(inGameProcess)
	    	    check();
            else 
                endGame(tempResult);
            GUIStyle fontStyle = new GUIStyle();  
	        fontStyle.alignment = TextAnchor.UpperCenter;  
	        fontStyle.normal.background = null;    //设置背景填充  
	        fontStyle.normal.textColor= new Color(120F/256,190F/256,197F/256);   //设置字体颜色  
	        fontStyle.fontSize = Screen.width/36;       //字体大小  
	    	GUI.Label(new Rect(Screen.height/10, Screen.height/10, LabelWidth, LabelHeight),"Player O: " + Owin ,fontStyle);
	    	GUI.Label(new Rect(Screen.height/10, Screen.height/6, LabelWidth, LabelHeight),"Player X: " + Xwin,fontStyle);
    	}
    	
	    	    
    }
    void endGame(string winner) {
    	print("endGame");
        inGameProcess = false;
        GUIStyle fontStyle = new GUIStyle();  
        fontStyle.alignment = TextAnchor.UpperCenter;  
        fontStyle.normal.background = null;    //设置背景填充  
        fontStyle.normal.textColor= new Color(120F/256,190F/256,197F/256);   //设置字体颜色  
        fontStyle.fontSize = Screen.width/36;       //字体大小  
        GUIStyle buttonStyle = new GUIStyle();  
        buttonStyle.normal.background = null;    //设置背景填充  
        buttonStyle.normal.textColor= new Color(120F/256,190F/256,197F/256);
        buttonStyle.alignment = TextAnchor.MiddleCenter;
        buttonStyle.fontSize = Screen.width/36;
        if(winner == "nobody") {
            GUI.Label(new Rect((Screen.width - LabelWidth)/2, Screen.height/10, LabelWidth, LabelHeight), 
                        "The game has drawn.",fontStyle);
        }
        if(winner == "player1") {
            GUI.Label(new Rect((Screen.width - LabelWidth)/2, Screen.height/10, LabelWidth, LabelHeight), 
                        "Player O win the game! ",fontStyle);
        }
        if(winner == "player2") {
            GUI.Label(new Rect((Screen.width - LabelWidth)/2, Screen.height/10, LabelWidth, LabelHeight), 
                        "Player X win the game! ",fontStyle);
        }
        if(GUI.Button(new Rect((Screen.width - LabelWidth)/2, Screen.height-Screen.height/5, LabelWidth, 40),"CLICK HERE TO CONTINUE",buttonStyle))
            reset();

    }
    void check() {
    	//检查每行
    	for(int i = 0; i < 3; i++) {
    		if(state[i,0] == state[i,1] && state[i,1] == state[i,2]) {
    			if(state[i,0] == 1) {
    				tempResult = "player1";
    			}
    			else if(state[i,0] == 2) {
    				tempResult = "player2";
    			}
    		}
    	}
    	//检查每列
    	for(int i = 0; i < 3; i++) {
    		if(state[0,i] == state[1,i] && state[1,i] == state[2,i]) {
    			if(state[0,i] == 1) {
    				tempResult = "player1";
    			}
    			else if(state[0,i] == 2) {
    				tempResult = "player2";
    			}
    		}
    	}
    	//检查对角线
    	if(state[0,0] == state[1,1] && state[1,1] == state[2,2]) {
    		if(state[0,0] == 1) {
    			tempResult = "player1";
    		}
    		else if(state[0,0] == 2) {
    			tempResult = "player2";
    		}
    	}

    	if(state[0,2] == state[1,1] && state[1,1] == state[2,0]) {
    		if(state[0,2] == 1) {
    			tempResult = "player1";
    		}
    		else if(state[0,2] == 2) {
    			tempResult = "player2";
    		}
    	}
    	if(inGameProcess && tempResult == "" && count == 9) {
    		tempResult = "nobody";
        }
        if(tempResult != "") {
        	print ("test");
        	if(tempResult == "player1") {
        		Owin++;
        	}
        	else if (tempResult == "player2") {
        		Xwin++;
        	}
            endGame(tempResult);
        }
    }
}