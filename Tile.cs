using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

// 퍼즐 타일 하나의 동작을 제어하는 클래스
public class Tile : MonoBehaviour, IPointerClickHandler
{
	private	TextMeshProUGUI	textNumeric; // 숫자 텍스트 UI
	private	Board board; // 타일이 속한 보드
	private	Vector3	correctPosition; // 올바른 위치(초기 배치 위치)

	public	bool IsCorrected { private set; get; } = false; // 현재 위치가 정답 위치인지 여부

	private	int numeric;
	public	int Numeric
	{
		set
		{
			numeric = value;
			textNumeric.text = numeric.ToString(); // 숫자 설정 및 표시
		}
		get => numeric;
	}
	// 타일 초기 설정
	/// <param name="board">연결된 보드 객체</param>
	/// <param name="hideNumeric">마지막 타일 번호 (숨길 숫자)</param>
	/// <param name="numeric">현재 타일 숫자</param>
 
	public void Setup(Board board, int hideNumeric, int numeric)
	{
		this.board	= board;
		textNumeric = GetComponentInChildren<TextMeshProUGUI>();

		Numeric = numeric;
  		// 마지막 타일은 빈 칸으로 처리
		if ( Numeric == hideNumeric )
		{
			GetComponent<UnityEngine.UI.Image>().enabled = false; // 타일 배경 비활성화
			textNumeric.enabled = false; // 숫자 숨김
		}
	}
	// 현재 위치를 '정답 위치'로 설정 (게임 시작 시 호출됨)
	public void SetCorrectPosition()
	{
		correctPosition = GetComponent<RectTransform>().localPosition;
	}
	// 마우스로 클릭했을 때 호출됨
	public void OnPointerClick(PointerEventData eventData)
	{
		// 보드에 이동 요청
		board.IsMoveTile(this);
	}
	// 주어잔 위치로 부드럽게 이동
	public void OnMoveTo(Vector3 end)
	{
		StartCoroutine("MoveTo", end);
	}
	// 코루틴 : 타일이 애니메이션으로 이동
	private IEnumerator MoveTo(Vector3 end)
	{
		float	current  = 0;
		float	percent  = 0;
		float	moveTime = 0.1f;
		Vector3	start	 = GetComponent<RectTransform>().localPosition;

		while ( percent < 1 )
		{
			current += Time.deltaTime;
			percent = current / moveTime;
			// 선형 보간을 통해 위치 갱신
			GetComponent<RectTransform>().localPosition = Vector3.Lerp(start, end, percent);

			yield return null;
		}
		// 이동 후 현재 위치가 정답 위치인지 확인
		IsCorrected = correctPosition == GetComponent<RectTransform>().localPosition ? true : false;
		
  		//이동 후 게임 종료 조건 확인 
		board.IsGameOver();
	}
}

