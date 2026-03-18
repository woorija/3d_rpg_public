# 3D RPG
## 프로젝트 소개
![02_title](https://github.com/user-attachments/assets/d3a21df7-151a-49fa-a6e9-e29545eb73b7)

 - 작품명 : **3d rpg (가제)**
 - 장르 : **3d rpg**
 - 플랫폼 : `Windows`
 - 개발 기간 : `2023.07.01~2026.03.04`
 - 개발 환경 : `Unity 6000.3.09f1 (urp)`
 - 개발 언어 : `C#`
 - 개발 팀원
    - **1인 개발**
 - 소개 : rpg에 필요한 각종 기능 개발 경험 및 최적화 연구를 위해 진행한 1인 프로젝트입니다. 

## 주요 기능
 1. 어드레서블을 사용하여 원격 다운로드 시스템 구현
    - 어드레서블 매니저를 구현하여 중복된 값을 가지는 핸들이 생성되는 것을 방지
    - 원하는 시점에 필요한 에셋만 비동기 로딩으로 불러오는 기능 구현
    - 초기 앱 용량 및 메모리 사용량 절감
 2. 몬스터 ai로 행동트리 구현
    - 블랙보드에 SO를 사용하여 중복 데이터를 최소화
    - 별도의 러닝노드 변수 사용 및 노드 가중치를 부여하고 캔슬 이벤트를 만들어 불필요한 연산 최소화
    - 범용 노드와 커스텀 에디터를 사용하여 인스펙터에서 간단히 트리구조를 변경할 수 있도록 처리
 3. 풀 매니저를 구현하여 GC생성 최소화
    - 유니티 내장 풀 시스템을 사용하여 풀 생성 로직 단순화
    - 풀 데이터를 SO로 사용하여 유지보수 최적화
    - 커스텀 에디터를 사용한 풀 매니저 자동생성기능 구현으로 불필요한 코드 작성 시간 단축
 4. 인풋 시스템을 사용한 손쉬운 멀티플랫폼 인풋 기능 구현
    - UI를 사용한 키 리바인딩을 구현하여 유저가 편리하게 단축키를 변경할 수 있도록 구현
 5. 환경설정 기능 구현
    - 유저 하드웨어 스펙에 따라 자율적으로 환경 설정을 할 수 있도록 하여 사용자 경험 개선
 6. UI, 데이터, 로직을 분리하여 유지보수 편의성 향상
 7. 커스텀 에디터 개발
    - csv데이터를 딕셔너리DB를 담은 클래스 스크립트로 자동변환하여 SO를 자동생성하여 각 씬의 데이터에 자동으로 할당하는 기능 구현을 통해 세팅 시간 단축
    - 몬스터 행동트리의 노드를 교체 및 프리셋 설정을 할 수 있도록 행동트리와 각 노드에 에디트 기능을 구현하여 세팅 시간 단축
    - 직업별 스탯 데이터를 인스펙터에서 커스터마이징 할 수 있는 에디터를 구현하여 인스펙터에서 간단하게 원하는 스탯 가중치 부여 가능

## 프로젝트 설치 방법
1. [Unity Hub](https://unity.com/kr/download)를 설치한다.
2. `Unity 6000.3.09f1`버전의 에디터를 설치한다.
3. 저장소를 클론한다.
   ```bash
   git clone https://github.com/woorija/3d_rpg_public.git
   ```
4. Unity Hub에서 프로젝트를 실행한다.
5. 패키지 매니저를 열고 [DOTween Pro](https://assetstore.unity.com/packages/tools/visual-scripting/dotween-pro-32416)를 설치한다.
6. 패키지 매니저의 +버튼을 클릭하고 `install from git URL`를 선택하여 `https://github.com/Cysharp/UniTask.git?path=src/UniTask/Assets/Plugins/UniTask#2.1.0`를 입력하여 UniTask를 설치한다.
   
## 게임 실행
 1. [다운로드](https://drive.google.com/drive/folders/1BmH_ziBOMoVvFrHsOeNxG6PIKHQBI3Zz)
 2. zip 파일을 압축 해제하고 `3dtest.exe` 파일을 실행하면 게임을 플레이할 수 있습니다.
   - `!!!주의!!!` 폴더 내 다른 파일, 폴더를 옮겨서는 안됩니다.

## 게임 조작법
 - w,a,s,d로 이동, 마우스 움직임을 통해 시야 이동이 가능합니다.
 - esc키 입력시 세팅창을 오픈할 수 있습니다.
 - 키 설정에서 각 단축키를 확인할 수 있습니다.

## 스크린샷
<img width="480" height="270" alt="Image" src="https://github.com/user-attachments/assets/d97ce95f-4fe3-4441-8ae2-fad47739fcd4" />
<img width="480" height="270" alt="Image" src="https://github.com/user-attachments/assets/07bdb526-42b4-4883-8199-46bdf8f9e027" />
<img width="480" height="270" alt="Image" src="https://github.com/user-attachments/assets/b7150bbd-7d4e-4d1a-b745-5a9dfcc5451b" />
<img width="480" height="270" alt="Image" src="https://github.com/user-attachments/assets/d261cc26-48b8-4355-889e-39ce66861cc9" />
<img width="480" height="270" alt="Image" src="https://github.com/user-attachments/assets/d3a21df7-151a-49fa-a6e9-e29545eb73b7" />
<img width="480" height="270" alt="Image" src="https://github.com/user-attachments/assets/7b42043c-0614-424c-b5a2-944726d6bdfb" />

## 플레이 영상
[![02_title](https://github.com/user-attachments/assets/d3a21df7-151a-49fa-a6e9-e29545eb73b7)](https://youtu.be/2AEsu8yUs8M)

## 라이센스
```
This project uses the following third-party libraries:

UniTask
Copyright (c) 2019 Yoshifumi Kawai / Cysharp, Inc.
Licensed under the MIT License.
https://github.com/Cysharp/UniTask

KCC 간판체
한국저작권위원회, https://www.copyright.or.kr/main.do
```
