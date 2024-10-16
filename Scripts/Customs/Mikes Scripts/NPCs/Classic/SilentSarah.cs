using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

[CorpseName("the corpse of Silent Sarah")]
public class SilentSarah : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public SilentSarah() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Silent Sarah";
        Body = 0x191; // Human female body

        // Stats
        SetStr(145);
        SetDex(70);
        SetInt(30);
        SetHits(105);

        // Appearance
        AddItem(new LeatherCap() { Hue = 1120 });
        AddItem(new FemaleLeatherChest() { Hue = 1120 });
        AddItem(new Sandals() { Hue = 1170 });
        AddItem(new LeatherGloves() { Name = "Sarah's Stealthy Gloves" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
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
        DialogueModule greeting = new DialogueModule("I am Silent Sarah, the whisperer of shadows. What do you seek?");
        
        greeting.AddOption("Tell me about your health.",
            player => true,
            player => 
            {
                player.SendGump(new DialogueGump(player, new DialogueModule("My health is not your concern, for I am beyond the grasp of death.")));
            });

        greeting.AddOption("What is your job?",
            player => true,
            player => 
            {
                player.SendGump(new DialogueGump(player, new DialogueModule("I was once a murderer, a shadow in the night. Do you seek the darkness as well?")));
            });

        greeting.AddOption("What are your favorite kills in the shadows?",
            player => true,
            player => 
            {
                DialogueModule killStoriesModule = new DialogueModule("Ah, the tales of my shadows. Each kill tells a story. Would you like to hear about the Merchant of Misfortune or the Guard of the Glimmering Gate?");
                
                killStoriesModule.AddOption("Tell me about the Merchant of Misfortune.",
                    pl => true,
                    pl => 
                    {
                        DialogueModule merchantModule = new DialogueModule("The Merchant was a greedy soul, hoarding riches while others starved. One night, I crept into his chamber, cloaked in darkness, and... *whispers* I took from him what he valued most. His screams were music to my ears.");
                        
                        merchantModule.AddOption("What happened next?",
                            p => true,
                            p => 
                            {
                                player.SendGump(new DialogueGump(player, new DialogueModule("He became a cautionary tale in the streets—a warning to those who let greed consume them. I made sure he was never found.")));
                            });

                        merchantModule.AddOption("Do you regret it?",
                            p => true,
                            p => 
                            {
                                player.SendGump(new DialogueGump(player, new DialogueModule("Regret? No, I thrive on the fear I sow. It's a powerful feeling, knowing I am the whisper behind the darkness.")));
                            });

                        player.SendGump(new DialogueGump(player, merchantModule));
                    });

                killStoriesModule.AddOption("Tell me about the Guard of the Glimmering Gate.",
                    pl => true,
                    pl => 
                    {
                        DialogueModule guardModule = new DialogueModule("The Guard was ever vigilant, standing tall against those who wished to cross into the realm of shadows. He thought himself untouchable. One night, I slipped past his defenses, and in a moment of silence, I... *pauses* I ended his watch forever.");
                        
                        guardModule.AddOption("What did you take from him?",
                            p => true,
                            p => 
                            {
                                player.SendGump(new DialogueGump(player, new DialogueModule("His sword, a prized possession, whispered tales of valor. But in my hands, it became a tool of treachery. I left him with nothing.")));
                            });

                        guardModule.AddOption("Was it worth it?",
                            p => true,
                            p => 
                            {
                                player.SendGump(new DialogueGump(player, new DialogueModule("Every life taken adds to my power, my legacy. The worth lies in the fear I instill.")));
                            });

                        player.SendGump(new DialogueGump(player, guardModule));
                    });

                player.SendGump(new DialogueGump(player, killStoriesModule));
            });

        greeting.AddOption("What whispers do you speak of?",
            player => true,
            player => 
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    player.SendGump(new DialogueGump(player, new DialogueModule("I have no reward right now. Please return later.")));
                }
                else
                {
                    lastRewardTime = DateTime.UtcNow;
                    player.AddToBackpack(new FootwearSlotChangeDeed());
                    player.SendGump(new DialogueGump(player, new DialogueModule("Whispers are the voices of the departed, tales of sorrow and regret. If you listen carefully, you might just hear them. For your patience, I grant you this gift.")));
                }
            });

        greeting.AddOption("What about light and darkness?",
            player => true,
            player => 
            {
                player.SendGump(new DialogueGump(player, new DialogueModule("The light once shone bright in my soul, but it was smothered by my actions. Darkness can only be understood when you've been in the light. Tell me, do you fear the light or embrace it?")));
            });

        greeting.AddOption("What does freedom mean to you?",
            player => true,
            player => 
            {
                player.SendGump(new DialogueGump(player, new DialogueModule("True freedom is not just the absence of shackles, but the liberation of the soul. I've tasted both and still yearn for the latter. What does freedom mean to you?")));
            });

        return greeting;
    }

    public SilentSarah(Serial serial) : base(serial) { }

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
