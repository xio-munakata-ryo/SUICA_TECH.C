using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingManager : MonoBehaviour
{
    const string SAVE_DATA_NAME = "main_rank";

    [System.Serializable]
    public class Rank
    {
        public string Name;
        public int Score;

        [System.NonSerialized]
        public bool IsRecent;

        public Rank(string name, int score, bool isRecent)
        {
            this.Name = name;
            this.Score = score;
            this.IsRecent = isRecent;
        }

        public void ClearRecent()
        {
            IsRecent = false;
        }
    }

    [System.Serializable]
    public class RankListWrapper
    {
        public List<Rank> ListRank = new List<Rank>();
    }

    private RankListWrapper _listRankWrapper = new RankListWrapper();


    [SerializeField] private RankingNode _prefabNode;



    private List<RankingNode> _listRankingNodes = new List<RankingNode>();

    public void ClearRankingView()
    {
        foreach (var node in _listRankingNodes)
        {
            Destroy(node.gameObject);
        }

        _listRankingNodes.Clear();

        foreach (var rank in _listRankWrapper.ListRank)
        {
            if (rank.IsRecent) rank.ClearRecent();
        }
    }

    public void AddRank(string name, int score, bool isRecent = false)
    {
        _listRankWrapper.ListRank.Add(new Rank(name, score, isRecent));

        SaveLoadManager.Save<RankListWrapper>(SAVE_DATA_NAME, _listRankWrapper);
    }

    public void InitRankingView()
    {
        _listRankWrapper.ListRank.Sort((x, y) => y.Score - x.Score);
        // 上限5個まで表示
        if (_listRankWrapper.ListRank.Count > 5)
        {
            _listRankWrapper.ListRank.RemoveRange(5, _listRankWrapper.ListRank.Count - 5);
        }

        for (int i = 0; i < _listRankWrapper.ListRank.Count; i++)
        {
            var rank = _listRankWrapper.ListRank[i];
            RankingNode node = Instantiate(_prefabNode);
            node.SetNameTxt(rank.Name);
            node.SetPointTxt(rank.Score);
            if (rank.IsRecent) node.SetFlashBackImage();

            _listRankingNodes.Add(node);

            node.transform.SetParent(this.transform);
        }
    }

    private void Awake()
    {
        _listRankWrapper = SaveLoadManager.Load<RankListWrapper>(SAVE_DATA_NAME, _listRankWrapper);
    }
}
