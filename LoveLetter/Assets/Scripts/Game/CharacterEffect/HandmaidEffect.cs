
public class HandmaidEffect : ICharacterEffect
{
    public CharacterType CharacterType => CharacterType.Handmaid;

    public bool DoEffect(PlayerScript player, int cardId)
    {
        player.PlayerStatus = PlayerStatus.Protected;
        MonoHelper.Instance.SetActionText(player.PlayerName + " is protected till his next turn");

        GameManager.instance.CardEffectPlayed(cardId, player);

        return true;
    }
}