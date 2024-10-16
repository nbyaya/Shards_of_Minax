using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Gumps;
using Server.Network;

[CorpseName("the corpse of Morgan Le Fay")]
public class MorganLeFay : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public MorganLeFay() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Morgan Le Fay";
        Body = 0x191; // Human female body

        // Stats
        Str = 80;
        Dex = 50;
        Int = 110;
        Hits = 60;

        // Appearance
        AddItem(new Robe() { Hue = 1155 });
        AddItem(new Boots() { Hue = 1155 });
        AddItem(new WizardsHat() { Hue = 1155 });
        AddItem(new Spellbook() { Name = "Morgan's Grimoire" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(Female);
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
        DialogueModule greeting = new DialogueModule("I am Morgan Le Fay, the witch of these woods. How may I assist you, seeker of knowledge?");

        greeting.AddOption("Tell me about your health.",
            player => true,
            player => player.SendGump(new DialogueGump(player, new DialogueModule("My health is as resilient as the forest itself. Like the trees that weather storms, I endure."))));

        greeting.AddOption("What is your job?",
            player => true,
            player => player.SendGump(new DialogueGump(player, new DialogueModule("I am a keeper of ancient knowledge and a practitioner of arcane arts. My craft is both a blessing and a burden."))));

        greeting.AddOption("What do you know about virtues?",
            player => true,
            player => player.SendGump(new DialogueGump(player, new DialogueModule("The virtues, those guiding lights, they shape our destinies, don't they? Each one is a path leading to greater truths."))));

        greeting.AddOption("Tell me more about resilience.",
            player => true,
            player => player.SendGump(new DialogueGump(player, new DialogueModule("Resilience is more than mere physical strength. It's the spirit's ability to endure and grow from challenges. Like the phoenix, we can rise anew from the ashes."))));

        greeting.AddOption("What can you tell me about arcane arts?",
            player => true,
            player => player.SendGump(new DialogueGump(player, new DialogueModule("Arcane arts are not merely spells. They are a deep connection to the world's energies, a bond few truly understand. Through them, I shape the fabric of reality."))));

        greeting.AddOption("What happens when one is consumed by desire?",
            player => true,
            player => player.SendGump(new DialogueGump(player, new DialogueModule("I've seen many consumed by their desires, lost in the abyss of their own making. Desire is a double-edged sword; it can motivate or ruin."))));

        greeting.AddOption("What is the curse you mentioned?",
            player => true,
            player =>
            {
                DialogueModule curseModule = new DialogueModule("Ah, the curse. For one who proves themselves worthy, I might share its tale and perhaps offer a token of my esteem. But be warned; curses hold lessons as well as burdens.");
                curseModule.AddOption("Can you reward me?",
                    p => CanReward(p),
                    p =>
                    {
                        if (RewardPlayer(p))
                            p.SendMessage("You have been rewarded for your perseverance. Use it wisely.");
                        else
                            p.SendMessage("I have no reward right now. Please return later.");
                    });
                player.SendGump(new DialogueGump(player, curseModule));
            });

        greeting.AddOption("What is your history?",
            player => true,
            player => player.SendGump(new DialogueGump(player, new DialogueModule("My history is woven into the very fabric of these woods. I have walked the paths of magic long before many were born, learning from the whispers of the ancients."))));

        greeting.AddOption("Do you have any prophecies?",
            player => true,
            player => player.SendGump(new DialogueGump(player, new DialogueModule("Prophecies are like shadows; they hint at possibilities, but never reveal the full truth. One must discern their meaning through experience."))));

        greeting.AddOption("Can you teach me magic?",
            player => true,
            player => player.SendGump(new DialogueGump(player, new DialogueModule("Teaching magic requires trust and commitment. Magic is not just words; it's understanding the world around you. What do you seek to learn?"))));

        greeting.AddOption("What do you think of fate?",
            player => true,
            player => player.SendGump(new DialogueGump(player, new DialogueModule("Fate is a tapestry woven by countless hands. Some believe we are mere threads, while others see themselves as the weavers. What are your thoughts?"))));

        greeting.AddOption("What are the dangers of this forest?",
            player => true,
            player => player.SendGump(new DialogueGump(player, new DialogueModule("This forest is alive with magic, but not all of it is benevolent. Beware the creatures that lurk in the shadows; they guard secrets best left undisturbed."))));

        greeting.AddOption("Can you tell me about the ancient knowledge?",
            player => true,
            player => player.SendGump(new DialogueGump(player, new DialogueModule("Ancient knowledge is a treasure, often hidden behind trials. It speaks of truths forgotten and powers lost. Are you ready to seek it?"))));

        greeting.AddOption("What do you think of the adventurers that pass through?",
            player => true,
            player => player.SendGump(new DialogueGump(player, new DialogueModule("Adventurers are like storms; they bring change, but also chaos. Some seek glory, while others seek redemption. Each has a story worth hearing."))));

        greeting.AddOption("What wisdom can you share?",
            player => true,
            player => player.SendGump(new DialogueGump(player, new DialogueModule("Wisdom often comes from listening more than speaking. Each encounter holds a lesson, and each choice leads to growth. What wisdom do you seek?"))));

        return greeting;
    }

    private bool CanReward(PlayerMobile player)
    {
        return DateTime.UtcNow - lastRewardTime >= TimeSpan.FromMinutes(10);
    }

    private bool RewardPlayer(PlayerMobile player)
    {
        if (CanReward(player))
        {
            player.AddToBackpack(new TinkeringAugmentCrystal()); // Give the reward
            lastRewardTime = DateTime.UtcNow; // Update the timestamp
            return true;
        }
        return false;
    }

    public MorganLeFay(Serial serial) : base(serial) { }

    public override void Serialize(GenericWriter writer)
    {
        base.Serialize(writer);
        writer.Write(0); // version
        writer.Write(lastRewardTime);
    }

    public override void Deserialize(GenericReader reader)
    {
        base.Deserialize(reader);
        int version = reader.ReadInt();
        lastRewardTime = reader.ReadDateTime();
    }
}
