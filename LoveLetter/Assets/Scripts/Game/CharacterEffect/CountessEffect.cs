
public class CountessEffect : ICharacterEffect
{
    public CharacterType CharacterType => CharacterType.Countess;

    public bool DoEffect(PlayerScript player, int cardId)
    {
        MonoHelper.Instance.SetActionText("Countess has no special effect");
        GameManager.instance.CardEffectPlayed(cardId, player);
        return true;
    }
}

