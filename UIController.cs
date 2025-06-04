using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

// 게임 UI(결과 패널)를 제어하는 클래스
public class UIController : MonoBehaviour
{
	[SerializeField]
	private	GameObject resultPanel; // 게임 클리어 시 나타나는 결과 패널
	[SerializeField]
	private	TextMeshProUGUI	textPlaytime; //플레이 타임 텍스트
	[SerializeField]
	private	TextMeshProUGUI	textMoveCount; //이동 횟수 텍스트
	[SerializeField]
	private	Board board; // Board 스크립트 참조(Playtime과 MoveCount에 접근하기 위해)

	// 게임 클리어 시 결과 패널을 표시하고 정보 출력
	public void OnResultPanel()
	{
		resultPanel.SetActive(true);

		textPlaytime.text = $"PLAY TIME : {board.Playtime/60:D2}:{board.Playtime%60:D2}"; // 플레이 타임 출력
		textMoveCount.text = "MOVE COUNT : "+board.MoveCount; // 이동 횟수 출력
	}

 	// 재시작 버튼 클릭 시 현재 씬을 다시 로드
	public void OnClickRestart()
	{
 		// 현재 활성화된 씬을 다시 로드하여 게임 재시작
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}

