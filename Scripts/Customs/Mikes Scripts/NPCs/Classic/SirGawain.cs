using System;
using Server;
using Server.Mobiles;
using Server.Items;

public class SirGawain : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public SirGawain() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Sir Gawain";
        Body = 0x190; // Human male body

        // Stats
        SetStr(163);
        SetDex(68);
        SetInt(29);
        SetHits(118);

        // Appearance
        AddItem(new PlateChest() { Hue = 1190 });
        AddItem(new PlateLegs() { Hue = 1190 });
        AddItem(new PlateGloves() { Hue = 1190 });
        AddItem(new PlateArms() { Hue = 1190 });
        AddItem(new Bascinet() { Hue = 1190 });
        
        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        // Speech Hue
        SpeechHue = 0; // Default speech hue

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
        DialogueModule greeting = new DialogueModule("I am Sir Gawain, once a proud knight of the realm. How may I assist you?");

        greeting.AddOption("Tell me about your past.",
            player => true,
            player =>
            {
                DialogueModule pastModule = new DialogueModule("My body may be whole, but my spirit is broken. Valor is a fading memory.");
                pastModule.AddOption("What happened to you?",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateTimePortalModule()));
                    });
                player.SendGump(new DialogueGump(player, pastModule));
            });

        greeting.AddOption("Do you seek your lost amulet?",
            player => true,
            player =>
            {
                DialogueModule amuletModule = new DialogueModule("It was a pendant with a blue gem at its center, stolen during a skirmish. If you find it, return it to me.");
                amuletModule.AddOption("What do you offer as a reward?",
                    pl => true,
                    pl =>
                    {
                        if (DateTime.UtcNow - lastRewardTime < TimeSpan.FromMinutes(10))
                        {
                            pl.SendMessage("I have no reward right now. Please return later.");
                        }
                        else
                        {
                            pl.SendMessage("Ah, yes. I shall grant you a token of my gratitude.");
                            pl.AddToBackpack(new MaxxiaScroll()); // Give the reward
                            lastRewardTime = DateTime.UtcNow; // Update the timestamp
                        }
                    });
                player.SendGump(new DialogueGump(player, amuletModule));
            });

        greeting.AddOption("What do you know of Lancelot?",
            player => true,
            player =>
            {
                DialogueModule lancelotModule = new DialogueModule("He was once my closest ally. Betrayed by ambition, he cannot be trusted. If you seek him, be wary.");
                player.SendGump(new DialogueGump(player, lancelotModule));
            });

        return greeting;
    }

    private DialogueModule CreateTimePortalModule()
    {
        DialogueModule portalModule = new DialogueModule("It began on a fateful night when I stumbled upon a mysterious portal shimmering with arcane energy. Stepping through, I was pulled into a realm of chaos and despair.");
        portalModule.AddOption("What did you find on the other side?",
            pl => true,
            pl =>
            {
                DialogueModule otherSideModule = new DialogueModule("I found myself in a dark world ruled by Minax, a sorceress of unparalleled power. Her armies were relentless, and I was forced to serve her.");
                otherSideModule.AddOption("Why did you serve Minax?",
                    p => true,
                    p =>
                    {
                        DialogueModule serveModule = new DialogueModule("At first, I resisted, but Minax threatened the lives of my comrades. I had no choice but to obey her commands to protect them.");
                        serveModule.AddOption("What were your duties?",
                            plq => true,
                            plq =>
                            {
                                DialogueModule dutiesModule = new DialogueModule("I was tasked with gathering relics and enforcing her will across the realm. Each mission was a reminder of my lost honor.");
                                dutiesModule.AddOption("Did you ever try to escape?",
                                    pll => true,
                                    pll =>
                                    {
                                        DialogueModule escapeModule = new DialogueModule("Many times, but Minax's grip was too strong. I lost hope, wandering in a shadow of my former self.");
                                        escapeModule.AddOption("What changed?",
                                            plll => true,
                                            plll =>
                                            {
                                                DialogueModule changeModule = new DialogueModule("One day, I discovered a group of rebels plotting against her. I joined their cause, fueled by a flicker of hope for redemption.");
                                                changeModule.AddOption("Did you succeed?",
                                                    pllll => true,
                                                    pllll =>
                                                    {
                                                        DialogueModule successModule = new DialogueModule("In a fierce battle, we managed to weaken her forces. But in the chaos, I was once again caught in a portal, returning to this land, alone and haunted.");
                                                        pl.SendGump(new DialogueGump(pl, successModule));
                                                    });
                                                pl.SendGump(new DialogueGump(pl, changeModule));
                                            });
                                        pl.SendGump(new DialogueGump(pl, escapeModule));
                                    });
                                pl.SendGump(new DialogueGump(pl, dutiesModule));
                            });
                        pl.SendGump(new DialogueGump(pl, serveModule));
                    });
                pl.SendGump(new DialogueGump(pl, otherSideModule));
            });
        return portalModule;
    }

    public SirGawain(Serial serial) : base(serial) { }

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
