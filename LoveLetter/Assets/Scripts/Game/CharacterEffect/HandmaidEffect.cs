
public class HandmaidEffect : ICharacterEffect
{
    public CharacterType CharacterType => CharacterType.Handmaid;

    public bool DoEffect(PlayerScript player, int cardId)
    {
        player.PlayerStatus = PlayerStatus.Protected;
        Text.ActionSync("Handmaiden protects " + player.PlayerName + " till his/her next turn");

        GameManager.instance.CardEffectPlayed(cardId, player);

        return true;
    }
}