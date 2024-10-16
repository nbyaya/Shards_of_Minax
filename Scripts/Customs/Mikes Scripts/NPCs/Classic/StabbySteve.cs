using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

[CorpseName("the corpse of Stabby Steve")]
public class StabbySteve : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public StabbySteve() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Stabby Steve";
        Body = 0x190; // Human male body

        // Stats
        SetStr(160);
        SetDex(70);
        SetInt(20);
        SetHits(115);

        // Appearance
        AddItem(new BoneHelm() { Hue = 1172 });
        AddItem(new LongPants() { Hue = 1154 });
        AddItem(new Tunic() { Hue = 1120 });
        AddItem(new Boots() { Hue = 1908 });
        AddItem(new LeatherGloves() { Hue = 0, Name = "Steve's Slicing Gloves" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();
        FacialHairItemID = Race.RandomFacialHair(this);

        lastRewardTime = DateTime.MinValue;
    }

    public StabbySteve(Serial serial) : base(serial) { }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule();
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule()
    {
        DialogueModule greeting = new DialogueModule("What's yer business, traveler?");
        
        greeting.AddOption("Who are you?",
            player => true,
            player =>
            {
                player.SendGump(new DialogueGump(player, new DialogueModule("They call me Stabby Steve, but names don't mean much in my line of work.")));
            });
        
        greeting.AddOption("Tell me about your job.",
            player => true,
            player =>
            {
                player.SendGump(new DialogueGump(player, new DialogueModule("Murderin' is my trade, if ya gotta know.")));
            });
        
        greeting.AddOption("What's your favorite place to stab someone?",
            player => true,
            player =>
            {
                DialogueModule favoritePlacesModule = new DialogueModule("Ah, now yer talkin'! I’ve got a few places I fancy for a good ol’ stab. Where do ya want to start?");
                
                favoritePlacesModule.AddOption("The Whispering Forest",
                    pl => true,
                    pl => 
                    {
                        DialogueModule whisperingForestModule = new DialogueModule("The Whispering Forest, eh? It's got a dark charm to it. The trees seem to whisper secrets as ya stalk yer prey. There's somethin' satisfying about blendin' in with the shadows.");
                        
                        whisperingForestModule.AddOption("What makes it special?",
                            p => true,
                            p =>
                            {
                                p.SendGump(new DialogueGump(p, new DialogueModule("The rustling leaves hide the cries of the fallen, and if ya listen closely, ya can hear the echoes of their last moments.")));
                            });

                        whisperingForestModule.AddOption("Any memorable stabbings there?",
                            p => true,
                            p =>
                            {
                                p.SendGump(new DialogueGump(p, new DialogueModule("Ah, I once caught a merchant there, thinkin' he was safe in the woods. Didn't even see it comin'. The look on his face was priceless!")));
                            });

                        pl.SendGump(new DialogueGump(pl, whisperingForestModule));
                    });

                favoritePlacesModule.AddOption("The Dark Alley in Britain",
                    pl => true,
                    pl =>
                    {
                        DialogueModule darkAlleyModule = new DialogueModule("The Dark Alley in Britain is a classic! It’s where many go to settle scores. The shadows there are thick enough to hide even the most experienced blade.");
                        
                        darkAlleyModule.AddOption("Why do you like it?",
                            p => true,
                            p =>
                            {
                                p.SendGump(new DialogueGump(p, new DialogueModule("It's like a second home. The echoes of past deals gone wrong still linger in the air, and the fear of betrayal makes every stab feel like a dance.")));
                            });

                        darkAlleyModule.AddOption("Have you made allies there?",
                            p => true,
                            p =>
                            {
                                p.SendGump(new DialogueGump(p, new DialogueModule("Allies are few and far between, but I've had my share. We've shared blades and blood, but trust is a fragile thing in that line of work.")));
                            });

                        pl.SendGump(new DialogueGump(pl, darkAlleyModule));
                    });

                favoritePlacesModule.AddOption("The Ruins of the Old Castle",
                    pl => true,
                    pl =>
                    {
                        DialogueModule castleRuinsModule = new DialogueModule("The ruins hold stories of betrayal and revenge. It's a perfect backdrop for a stab, where the spirits of the betrayed linger, watchin' and waiting.");
                        
                        castleRuinsModule.AddOption("What happened there?",
                            p => true,
                            p =>
                            {
                                p.SendGump(new DialogueGump(p, new DialogueModule("Ah, many a tale, but one stands out. I had a rival there once, and let’s just say, the castle’s stones are now stained with his treachery.")));
                            });

                        castleRuinsModule.AddOption("Is it haunted?",
                            p => true,
                            p =>
                            {
                                p.SendGump(new DialogueGump(p, new DialogueModule("Haunted, indeed! Many claim to have seen the ghosts of those who met their end there. Makes the thrill all the more exciting, don’t ya think?")));
                            });

                        pl.SendGump(new DialogueGump(pl, castleRuinsModule));
                    });

                greeting.AddOption("What about the Abyss?",
                    playerq => true,
                    playerq =>
                    {
                        DialogueModule abyssModule = new DialogueModule("The Abyss is a place of darkness and despair. Perfect for those who want to make their mark without anyone knowin' about it.");
                        
                        abyssModule.AddOption("Why do you prefer darkness?",
                            p => true,
                            p =>
                            {
                                p.SendGump(new DialogueGump(p, new DialogueModule("In darkness, yer deeds go unseen. It’s liberatin’, like a dance with death itself. You can feel the thrill coursing through ya as you strike.")));
                            });

                        abyssModule.AddOption("Have you ever been caught?",
                            p => true,
                            p =>
                            {
                                p.SendGump(new DialogueGump(p, new DialogueModule("Once or twice, but I’ve always managed to slip away. It’s all about bein' one step ahead of the game.")));
                            });

                        playerq.SendGump(new DialogueGump(playerq, abyssModule));
                    });

                player.SendGump(new DialogueGump(player, favoritePlacesModule));
            });

        greeting.AddOption("What do you want from life?",
            player => true,
            player =>
            {
                player.SendGump(new DialogueGump(player, new DialogueModule("All I want is to live by my own rules, make my mark, and maybe, just maybe, find a place where betrayal doesn't exist.")));
            });

        greeting.AddOption("What do you think about betrayal?",
            player => true,
            player =>
            {
                player.SendGump(new DialogueGump(player, new DialogueModule("Betrayal is a bitter pill. It's taught me not to trust easily, but it can lead you to unexpected allies.")));
            });

        return greeting;
    }

    public override void Serialize(GenericWriter writer)
    {
        base.Serialize(writer);
        writer.Write((int)0); // version
        writer.Write(lastRewardTime);
    }

    public override void Deserialize(GenericReader reader)
    {
        base.Deserialize(reader);
        int version = reader.ReadInt();
        lastRewardTime = reader.ReadDateTime();
    }
}
