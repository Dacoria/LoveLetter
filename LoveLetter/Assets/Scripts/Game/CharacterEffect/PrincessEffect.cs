
public class PrincessEffect : ICharacterEffect
{
    public CharacterType CharacterType => CharacterType.Princess;

    public bool DoEffect(PlayerScript player, int cardId)
    {
        MonoHelper.Instance.SetActionText(player.PlayerName + " discarded the princess - instant lose");
        player.PlayerStatus = PlayerStatus.Intercepted;

        GameManager.instance.CardEffectPlayed(cardId, player);
        return true;
    }
}

