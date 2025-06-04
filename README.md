# Puzzle-game-proj
퍼즐 게임 개발 학부 프로젝트</br>
📌 이 저장소는 전체 Unity 퍼즐 프로젝트 중,
제가 직접 구현한 주요 C# 스크립트 3종을 정리한 저장소입니다.

전체 시연 영상 및 결과물은 👉 [노션 포트폴리오](https://magical-rate-172.notion.site/LostMemory-c7afecf2c7b6458eb5c45e199a6f8896) 에서 퍼즐게임 Lost Memort에서 확인하실 수 있습니다.

## 🎮 주요 기능

| 기능 | 설명 |
|------|------|
| 타일 셔플 | 게임 시작 시 자동 섞기 애니메이션 |
| 타일 클릭 | 빈 칸과 인접한 타일만 이동 가능 |
| 정답 체크 | 모든 타일이 제자리에 오면 게임 클리어 |
| 플레이 타임 | 초 단위로 플레이 시간 측정 |
| 이동 횟수 | 타일 이동 시마다 카운트 증가 |
| 결과 화면 | 클리어 시 결과 패널 노출 및 재시작 가능 |

---

## 🛠️ 사용 기술

- **Unity (v2020 이상)**
- **C# (MonoBehaviour 기반)**
- **UI System**
  - `GridLayoutGroup`
  - `TextMeshPro`
  - `EventSystem` + `IPointerClickHandler` 인터페이스

---
