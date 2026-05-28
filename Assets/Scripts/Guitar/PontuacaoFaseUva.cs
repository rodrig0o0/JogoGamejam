using UnityEngine;

public class PontuacaoFaseUva : MonoBehaviour
{
    [SerializeField] private int pontos;

    public int AddPontos(int addPontos)
    {
        return pontos += addPontos;
    }

    public int RedPontos(int redPontos)
    {
        return pontos -= redPontos;
    }
    
}
