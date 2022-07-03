
public class HandmaidEffect : CharacterEffect
{
    public override CharacterType CharacterType => CharacterType.Handmaid;

    public override bool DoEffect(PlayerScript player, int cardId)
    {
        player.PlayerStatus = PlayerStatus.Protected;
        Textt.ActionSync("Handmaiden protects " + player.PlayerName + " till his/her next turn");

        GameManager.instance.CardEffectPlayed(cardId, player.PlayerId);

        return true;
    }
}