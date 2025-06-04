using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// 퍼즐 보드의 동작을 관리하는 클래스

public class Board : MonoBehaviour
{
	[SerializeField]
	private	GameObject	tilePrefab;								// 숫자 타일 프리팹
	[SerializeField]
	private	Transform	tilesParent;							// 타일이 배치되는 "Board" 오브젝트의 Transform

	private	List<Tile>	tileList;								// 생성한 타일 정보 저장

	private	Vector2Int	puzzleSize = new Vector2Int(4, 4);		// 4x4 퍼즐
	private	float		neighborTileDistance = 102;				// 인접한 타일 사이의 거리. 별도로 계산할 수도 있다.

	public	Vector3		EmptyTilePosition { set; get; }			// 빈 타일의 위치
	public	int			Playtime { private set; get; } = 0;		// 게임 플레이 시간
	public	int			MoveCount { private set; get; } = 0;	// 이동 횟수



/// 게임 시작 시 실행되는 초기화 코루틴

	private IEnumerator Start()
	{
		tileList = new List<Tile>();

		SpawnTiles(); // 타일 생성

		UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(tilesParent.GetComponent<RectTransform>());  // UI 배치 강제 갱신

		// 현재 프레임이 종료될 때까지 대기(다음 프레임 까지 대기)
		yield return new WaitForEndOfFrame();

		// 각 타일의 정답 위치 설정
		tileList.ForEach(x => x.SetCorrectPosition());

		StartCoroutine("OnSuffle"); // 셔플 시작
  
		// 게임시작과 동시에 플레이시간 초 단위 연산
		StartCoroutine("CalculatePlaytime");
	}

// 퍼즐 타일 생성

	private void SpawnTiles()
	{
		for ( int y = 0; y < puzzleSize.y; ++ y )
		{
			for ( int x = 0; x < puzzleSize.x; ++ x )
			{
   				// 타일 생성 및 부모에 배치
				GameObject clone = Instantiate(tilePrefab, tilesParent);
				Tile tile = clone.GetComponent<Tile>();
				// 타일 초기 설정 (보드 참조, 총 타일 수, 현재 타일 번호)
				tile.Setup(this, puzzleSize.x * puzzleSize.y, y * puzzleSize.x + x + 1);

				tileList.Add(tile);
			}
		}
	}

 	// 퍼즐 셔플 (타일 순서를 임의로 섞음)
	private IEnumerator OnSuffle()
	{
		float current	= 0;
		float percent	= 0;
		float time		= 1.5f;

		while ( percent < 1 )
		{
			current += Time.deltaTime;
			percent = current / time;

   			// 랜덤한 타일을 리스트의 맨 뒤로 이동 (시각적 순서 변경)
			int index = Random.Range(0, puzzleSize.x * puzzleSize.y);
			tileList[index].transform.SetAsLastSibling();

			yield return null;
		}

		// 원래 셔플 방식은 다른 방식이었는데 UI, GridLayoutGroup을 사용하다보니 자식의 위치를 바꾸는 것으로 설정
		// 그래서 현재 타일리스트의 마지막에 있는 요소가 무조건 빈 타일
		EmptyTilePosition = tileList[tileList.Count-1].GetComponent<RectTransform>().localPosition;
	}

 	// 타일이 이동 가능한 거리인지 확인하고 이동 처리
	public void IsMoveTile(Tile tile)
	{	
 		//빈 공간과 인접한 경우에만 이동 가능
		if ( Vector3.Distance(EmptyTilePosition, tile.GetComponent<RectTransform>().localPosition) == neighborTileDistance)
		{
			Vector3 goalPosition = EmptyTilePosition;

   			//빈 공간 위치를 현재 타일 위치로 갱신
			EmptyTilePosition = tile.GetComponent<RectTransform>().localPosition;
			// 타일 이동 처리
			tile.OnMoveTo(goalPosition);

			// 타일을 이동할 때마다 이동 횟수 증가
			MoveCount ++;
		}
	}

 	// 게임 클리어 여부 확인
	public void IsGameOver()
	{	
 		// 올바른 위치에 있는 타일들을 모두 찾음
		List<Tile> tiles = tileList.FindAll(x => x.IsCorrected == true);

		Debug.Log("Correct Count : "+tiles.Count);

  		// 마지막 빈 칸 제외 모든 타일이 제자리에 있을 때 클리어
		if ( tiles.Count == puzzleSize.x * puzzleSize.y - 1 )
		{
			Debug.Log("GameClear");
   
			// 게임 클리어했을 때 시간계산 중지
			StopCoroutine("CalculatePlaytime");
			// Board 오브젝트에 컴포넌트로 설정하기 때문에
			// 그리고 한번만 호출하기 때문에 변수를 만들지 않고 바로 호출..
			GetComponent<UIController>().OnResultPanel();
		}
	}

 	// 1초마다 플레이 시간을 누적하는 코루틴
  
	private IEnumerator CalculatePlaytime()
	{
		while ( true )
		{
			Playtime ++;

			yield return new WaitForSeconds(1);
		}
	}
}
