using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using JGame.Pool;
using JGame.Data;

namespace JGame
{
    [System.Serializable]
    public class GameInfo
    {
        // max team num. 0 ~ teamNum
        public int teamNum;

        // map path
        public string mapPath;
    }

    public class GameManager : MonoBehaviour
    {
        public CameraInput cameraInput;

        GameInfo gameInfo = null;

        public Map map;

        // all controllers
        protected List<Controller> allController = new List<Controller>();
        // current turn controller index
        public int curTurn = 0;

        // all teams
        List<Team> teams = new List<Team>();

        // current turn controller 
        Controller controller = null;

        // local user controller
        Controller _localController = null;

        public Controller localController {  get { return _localController; } }

        // is Initialized?
        public bool isInitialized = false;

        // instance
        private static GameManager _instance = null;
        public static GameManager instance
        {
            get
            {
                // if null create instance
                if (_instance == null) CreateInstance();

                return _instance;
            }
        }

        // Awake
        private void Awake()
        {
            if( _instance == null )
            {
                _instance = this;
            }

            StartCoroutine(InitializeGame());
        }

        private void Start()
        {
            if(cameraInput == null )
            {
                cameraInput = JUtil.FindComponent<CameraInput>("Camera");
            }

            StartCoroutine(UpdateGame());
        }

        private void Update()
        {
            // if not selected ui
            if(EventSystem.current.currentSelectedGameObject == null && EventSystem.current.IsPointerOverGameObject() == false )
            {
                ProcessInput();
            }
        }

        // Create Instance
        protected static void CreateInstance()
        {
            if (_instance != null) return;

            GameObject obj = new GameObject();

            if (obj != null)
            {
                obj.name = "Game Manager";
                _instance = obj.AddComponent<GameManager>();
            }
        }

        // Initialize Game
        IEnumerator InitializeGame()
        {
            // {{ @ Test
            if (gameInfo == null )
            {
                gameInfo = new GameInfo();
                gameInfo.mapPath = "Map/Default";
                gameInfo.teamNum = 2;
            }
            // }} @Test

            // load map
            LoadMap();
            yield return null;

            // Create Teams
            for (int i = 0;i< gameInfo.teamNum ; ++i)
            {
                CreateTeam(i);
            }
            yield return null;
            
            // one team = one controller
            for ( int i =0; i< gameInfo.teamNum ; ++i)
            {
                CreateController(i);
            }

            // my controller
            _localController = allController[0];
            _localController.CreateArlamObj();
            yield return null;

            // Create Hero
            // {{ @test            
            for ( int j = 0; j<2;++j)
            {
                for (int i = 0; i < 4; ++i)
                {
                    CreateHero(allController[j],i);

                    yield return null;
                }
            }
            // }} @test

            isInitialized = true;
            yield return null;
        }

        // create new controller
        protected Controller CreateController( int teamIndex)
        {
            GameObject Obj = ObjectPoolManager.instance.Pop( DataController.instance.GetDefaultController() );

            Obj.name = "Controller_" + teamIndex;

            var control = Obj.GetComponent<Controller>();

            control.SetTeam(GetTeam(teamIndex));
            control.transform.SetParent(null);

            return control;
        }
        
        // create new hero
        protected void CreateHero( Controller owner , int index )
        {
            SpawnPoint sp = GetBestSpawnPoint(owner.team.teamIndex);

            if (sp == null) return;

            GameObject Obj = ObjectPoolManager.instance.Pop( DataController.instance.GetDefaultHero() , sp.transform.position);

            Obj.name = "hero_" + index;

            var hero = Obj.GetComponent<Hero>();

            owner.AddHero(Obj);
            hero.SetOwner(owner);
            hero.SetPosition(sp.position);

            // {{ @Test
            HeroInfo info = new HeroInfo();
            info.name = "name" + Obj.name;
            info.soldiers[3] = info.soldiers[4] = info.soldiers[5] = 1;
            // }} @Test
            hero.InitializeFromInfo(info);
        }

        // get spawn point
        protected SpawnPoint GetBestSpawnPoint(int teamIndex)
        {
            if (map == null)
            {
                return null;
            }

            for( int i = 0; i<map.spawnPoints.Count; ++i )
            {
                if( map.spawnPoints[i].teamIndex == teamIndex )
                {
                    if( map.GetTile(map.spawnPoints[i].position).actor == null )
                    {
                        return map.spawnPoints[i];
                    }
                }
            }

            return null;
        }

        // create new team
        protected Team CreateTeam(int index)
        {
            var team = new Team();

            team.teamIndex = index;
            teams.Add(team);

            return team;
        }

        protected Team GetTeam( int index )
        {
            int i = teams.FindIndex(item => item.teamIndex == index);
            if ( i == -1 )
            {
                return CreateTeam(index);
            }

            return teams[i];
        }

        // load map
        protected void LoadMap()
        {
            if( map == null )
            {
                map = JUtil.FindComponent<Map>();
            }

            if (map == null) return;

            if (gameInfo != null)
            {
                map.Initialize(gameInfo.mapPath);
            }
        }

        // Game Update
        IEnumerator UpdateGame()
        {
            // wait data controller initialize
            while( !DataController.instance.isReady )
            {
                yield return null;
            }

            //yield return new WaitForSeconds(0.1f);

            // wait initialize
            while( !isInitialized )
            {
                yield return null;
            }

            yield return new WaitForSeconds(0.1f);

            curTurn = -1;

            while ( true )
            {
                // get next turn controller
                controller = GetNextTurnController();

                // set controller auto
                controller.isAuto = (controller != localController);

                // begin turn
                controller.BeginTurn();
                yield return null;

                // update turn
                IEnumerator e = controller.CalcTurn();                
                while (e.MoveNext())
                {
                    yield return e.Current;
                }

                // end turn
                controller.EndTurn();
                yield return new WaitForSeconds( 0.5f );

                // game over?
                if ( CheckGameOver() )
                {
                    yield break;
                }
                yield return null;
            }
        }

        protected Controller GetNextTurnController()
        {
            curTurn += 1;

            // loop index
            if (curTurn < 0 || curTurn >= allController.Count )
            {
                curTurn = 0;
            }

            return allController[curTurn];
        }

        // game is over?
        protected bool CheckGameOver()
        {
            return false;
        }

        // process input
        protected bool ProcessInput()
        {
            if (cameraInput.ProcessInput())
            {
                return true;
            }

            if( localController != null && localController.ProcessInput() )
            {
                return true;
            }

            return false;
        }

        public void AddController(Controller ctr)
        {
            if (allController.Find(item => item == ctr) != null)
            {
                return;
            }

            allController.Add(ctr);
        }

        public void RemoveController(Controller ctr)
        {
            allController.Remove(ctr);
        }
    }
}
