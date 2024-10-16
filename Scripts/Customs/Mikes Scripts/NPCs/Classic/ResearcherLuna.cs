using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;

[CorpseName("the corpse of Researcher Luna")]
public class ResearcherLuna : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public ResearcherLuna() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Researcher Luna";
        Body = 0x191; // Human female body

        // Stats
        SetStr(78);
        SetDex(62);
        SetInt(105);
        SetHits(63);

        // Appearance
        AddItem(new ShortPants() { Hue = 1154 });
        AddItem(new Tunic() { Hue = 1107 });
        AddItem(new Sandals() { Hue = 1152 });
        AddItem(new Bonnet() { Hue = 1122 });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(Female);
        HairHue = Race.RandomHairHue();

        // Speech Hue
        SpeechHue = 0; // Default speech hue

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
    }

	public ResearcherLuna(Serial serial) : base(serial)
	{
	}

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule();
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule()
    {
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Researcher Luna, a scientist of Britannia. How may I assist you?");
        
        greeting.AddOption("Tell me about your research on the double moons.",
            player => true,
            player =>
            {
                DialogueModule moonsModule = new DialogueModule("Ah, the double moons! They are a fascinating subject of study. Did you know that they are known as Lunas and Nox? Their influence on the tides and magic is profound.");
                
                moonsModule.AddOption("What do you mean by their influence?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule influenceModule = new DialogueModule("The alignment of the moons affects the flow of magic across the land. Lunas, the brighter moon, amplifies healing spells, while Nox, the darker one, enhances destructive magic. It's a delicate balance.");
                        
                        influenceModule.AddOption("That's intriguing! How did you discover this?",
                            p => true,
                            p =>
                            {
                                DialogueModule discoveryModule = new DialogueModule("Through extensive research and observations, I noted patterns during lunar cycles. I documented numerous spells and their potency variations during different phases.");
                                
                                discoveryModule.AddOption("Can you share an example of this?",
                                    plq => true,
                                    plq =>
                                    {
                                        DialogueModule exampleModule = new DialogueModule("Certainly! During the full moon of Lunas, I cast a healing spell that restored far more health than during its waning phase. The effects were remarkable!");
                                        exampleModule.AddOption("That sounds powerful!",
                                            pw => true,
                                            pw =>
                                            {
                                                p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                                            });
                                        exampleModule.AddOption("How can I harness this knowledge?",
                                            pe => true,
                                            pe =>
                                            {
                                                DialogueModule harnessModule = new DialogueModule("To harness the moons' power, one must align their spells with the lunar phases. I can help guide you, if you wish.");
                                                harnessModule.AddOption("Yes, please guide me!",
                                                    pla => true,
                                                    pla =>
                                                    {
                                                        pla.SendMessage("To start, always observe the moon phase before casting spells. Each phase has a unique influence, so timing is crucial.");
                                                    });
                                                harnessModule.AddOption("Maybe another time.",
                                                    pla => true,
                                                    pla =>
                                                    {
                                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                                    });
                                                p.SendGump(new DialogueGump(p, harnessModule));
                                            });
                                        p.SendGump(new DialogueGump(p, exampleModule));
                                    });
                                discoveryModule.AddOption("How long have you been studying them?",
                                    plr => true,
                                    plr =>
                                    {
                                        pl.SendMessage("I have devoted the last five years of my life to this research. Every night spent under their light adds to our understanding.");
                                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                    });
                                p.SendGump(new DialogueGump(p, discoveryModule));
                            });
                        influenceModule.AddOption("I see! Are there dangers associated with them?",
                            plt => true,
                            plt =>
                            {
                                DialogueModule dangerModule = new DialogueModule("Indeed. The darker energies from Nox can lead to unpredictable effects. Spellcasters must exercise caution during its prominence.");
                                dangerModule.AddOption("What kind of unpredictable effects?",
                                    p => true,
                                    p =>
                                    {
                                        p.SendMessage("I've seen spells backfire spectacularly during the peak of Nox's influence. The chaotic energy can twist intentions, resulting in catastrophic outcomes.");
                                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                                    });
                                dangerModule.AddOption("That's unsettling!",
                                    ply => true,
                                    ply =>
                                    {
                                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                    });
                                player.SendGump(new DialogueGump(player, dangerModule));
                            });
                        player.SendGump(new DialogueGump(player, influenceModule));
                    });
                
                moonsModule.AddOption("What else do you know about them?",
                    playeru => true,
                    playeru =>
                    {
                        DialogueModule moreModule = new DialogueModule("The moons are not only beautiful but are tied to ancient prophecies. Legends speak of a celestial event when both moons align perfectly.");
                        moreModule.AddOption("What happens during this event?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule eventModule = new DialogueModule("It is said that during this rare alignment, a portal to the realm of dreams opens, allowing for visions of the future. Many have sought this event to gain foresight.");
                                eventModule.AddOption("Have you ever witnessed it?",
                                    p => true,
                                    p =>
                                    {
                                        p.SendMessage("Not yet, but I plan to document it when it occurs next. If you wish, I could send a message through the guild when the time approaches.");
                                    });
                                eventModule.AddOption("Sounds like an adventure!",
                                    pli => true,
                                    pli =>
                                    {
                                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                    });
                                player.SendGump(new DialogueGump(player, eventModule));
                            });
                        moreModule.AddOption("Are there any myths related to the moons?",
                            p => true,
                            p =>
                            {
                                DialogueModule mythsModule = new DialogueModule("Oh, many! One such myth tells of a goddess who rides the moons, bringing blessings or curses based on their phases. This belief influences many rituals in Britannia.");
                                mythsModule.AddOption("What kind of rituals?",
                                    pl => true,
                                    pl =>
                                    {
                                        p.SendMessage("Various rituals are performed during full moons, seeking blessings for harvest or protection. Others are held during new moons to appease the goddess and invite her favor.");
                                    });
                                mythsModule.AddOption("I’d love to learn more about these myths!",
                                    pl => true,
                                    pl =>
                                    {
                                        p.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                    });
                                player.SendGump(new DialogueGump(player, mythsModule));
                            });
                        player.SendGump(new DialogueGump(player, moreModule));
                    });

                player.SendGump(new DialogueGump(player, moonsModule));
            });

        greeting.AddOption("What else do you study?",
            player => true,
            player =>
            {
                DialogueModule otherResearchModule = new DialogueModule("Aside from the moons, I am also studying the interaction of magic with ancient artifacts. The artifacts tell stories of lost civilizations and their understanding of the arcane.");
                
                otherResearchModule.AddOption("Can you tell me about these artifacts?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule artifactsModule = new DialogueModule("Certainly! Many artifacts are imbued with magical properties, allowing for unique spells or abilities. Each artifact has its own history and significance.");
                        artifactsModule.AddOption("What kind of abilities?",
                            p => true,
                            p =>
                            {
                                p.SendMessage("Some artifacts can enhance a mage's power, while others may grant the ability to communicate with spirits or control elements.");
                                p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                            });
                        artifactsModule.AddOption("Are these artifacts easy to find?",
                            plo => true,
                            plo =>
                            {
                                pl.SendMessage("Most are hidden in ancient ruins, protected by powerful guardians. The journey to retrieve them can be perilous but rewarding.");
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        player.SendGump(new DialogueGump(player, artifactsModule));
                    });

                player.SendGump(new DialogueGump(player, otherResearchModule));
            });

        greeting.AddOption("Do you have any rewards for helping with your research?",
            player => true,
            player =>
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    player.SendMessage("I have no reward right now. Please return later.");
                }
                else
                {
                    player.AddToBackpack(new HidingAugmentCrystal());
                    lastRewardTime = DateTime.UtcNow;
                    player.SendMessage("Your aid would be invaluable. As a token of appreciation, I'll share a rare gem from my collection.");
                }
            });

        greeting.AddOption("Goodbye.",
            player => true,
            player =>
            {
                player.SendMessage("Safe travels, traveler.");
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
