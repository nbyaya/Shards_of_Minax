using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

[CorpseName("the corpse of Kai the Wild")]
public class KaiTheWild : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public KaiTheWild() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Kai the Wild";
        Body = 0x190; // Human male body

        // Stats
        SetStr(85);
        SetDex(60);
        SetInt(70);
        SetHits(85);

        // Appearance
        AddItem(new TribalMask() { Name = "Kai's Tribal Mask" });
        AddItem(new BoneArms() { Name = "Kai's Bone Arms" });
        AddItem(new BoneLegs() { Name = "Kai's Bone Legs" });
        AddItem(new BoneChest() { Name = "Kai's Bone Chest" });
        AddItem(new ShortSpear() { Name = "Kai's Tribal Spear" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        // Speech Hue
        SpeechHue = 0; // Default speech hue

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
    }

    public KaiTheWild(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("I am Kai the Wild, a tribal warrior of these lands. How can I help you?");

        greeting.AddOption("Tell me about your health.",
            player => true,
            player => { player.SendGump(new DialogueGump(player, new DialogueModule("My body is strong, and my spirit is fierce!"))); });

        greeting.AddOption("What is your job?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, new DialogueModule("I live by the ways of the tribe, protecting our lands and traditions."))); });

        greeting.AddOption("What do you know about virtues?",
            player => true,
            player =>
            {
                DialogueModule virtuesModule = new DialogueModule("The eight virtues are the foundation of our tribal life. Do you wish to learn more about them?");
                virtuesModule.AddOption("Yes, tell me more.",
                    pl => true,
                    pl => {
                        DialogueModule virtueDetails = new DialogueModule("Honesty, Compassion, Valor, Justice, Sacrifice, Honor, Spirituality, and Humility. Each virtue has its place in our lives.");
                        virtueDetails.AddOption("Tell me about Honesty.",
                            pl2 => true,
                            pl2 => { pl2.SendGump(new DialogueGump(pl2, new DialogueModule("Honesty is the bedrock of trust in our tribe. Without it, we cannot stand united."))); });
                        virtueDetails.AddOption("Tell me about Valor.",
                            pl2 => true,
                            pl2 => { pl2.SendGump(new DialogueGump(pl2, new DialogueModule("Valor is shown not just on the battlefield but also in standing up for what is right."))); });
                        virtueDetails.AddOption("Tell me about Spirituality.",
                            pl2 => true,
                            pl2 => { pl2.SendGump(new DialogueGump(pl2, new DialogueModule("Spirituality connects us to the land and the spirits of our ancestors. It guides our actions."))); });
                        virtueDetails.AddOption("What about the other virtues?",
                            pl2 => true,
                            pl2 => { pl2.SendGump(new DialogueGump(pl2, new DialogueModule("Justice, Sacrifice, Honor, and Humility complete the circle, reminding us of our responsibilities."))); });
                        pl.SendGump(new DialogueGump(pl, virtueDetails));
                    });
                virtuesModule.AddOption("No, maybe later.",
                    pl => true,
                    pl => { pl.SendGump(new DialogueGump(pl, CreateGreetingModule())); });
                player.SendGump(new DialogueGump(player, virtuesModule));
            });

        greeting.AddOption("What about the lands?",
            player => true,
            player => {
                DialogueModule landsModule = new DialogueModule("These lands have been the home of my ancestors for generations. They are sacred to us and hold many secrets.");
                landsModule.AddOption("What secrets do they hold?",
                    pl => true,
                    pl => { pl.SendGump(new DialogueGump(pl, new DialogueModule("The spirits of the land whisper stories of old. Many have been lost to time, but the wise among us remember."))); });
                landsModule.AddOption("Tell me about your favorite place.",
                    pl => true,
                    pl => {
                        DialogueModule placeModule = new DialogueModule("My favorite place is the Sacred Grove, where the ancient trees stand tall and the spirits dance under the moonlight.");
                        placeModule.AddOption("What happens there?",
                            pl2 => true,
                            pl2 => { pl2.SendGump(new DialogueGump(pl2, new DialogueModule("The tribe gathers for rituals and celebrations, connecting with the spirits and honoring our ancestors."))); });
                        placeModule.AddOption("I want to visit it.",
                            pl2 => true,
                            pl2 => { pl2.SendGump(new DialogueGump(pl2, new DialogueModule("It is a sacred site. Respect it, and the spirits will guide you."))); });
                        player.SendGump(new DialogueGump(player, placeModule));
                    });
                player.SendGump(new DialogueGump(player, landsModule));
            });

        greeting.AddOption("Tell me about your traditions.",
            player => true,
            player => {
                DialogueModule traditionsModule = new DialogueModule("Our traditions are passed down through the ages. One such tradition is the Festival of the Moon.");
                traditionsModule.AddOption("What is the Festival of the Moon?",
                    pl => true,
                    pl => {
                        DialogueModule festivalModule = new DialogueModule("The Festival of the Moon is a time of celebration and reflection. We gather to dance, sing, and remember the past while looking forward to the future.");
                        festivalModule.AddOption("What activities take place?",
                            pl2 => true,
                            pl2 => { pl2.SendGump(new DialogueGump(pl2, new DialogueModule("We perform dances, tell stories, and offer gifts to the spirits of our ancestors."))); });
                        festivalModule.AddOption("Can I participate?",
                            pl2 => true,
                            pl2 => { pl2.SendGump(new DialogueGump(pl2, new DialogueModule("Of course! All who respect our traditions are welcome."))); });
                        player.SendGump(new DialogueGump(player, festivalModule));
                    });
                traditionsModule.AddOption("What other traditions do you have?",
                    pl => true,
                    pl => { pl.SendGump(new DialogueGump(pl, new DialogueModule("We also honor the Harvest Festival and the Rite of Passage for young warriors."))); });
                player.SendGump(new DialogueGump(player, traditionsModule));
            });

        greeting.AddOption("Do you have a reward for me?",
            player => DateTime.UtcNow - lastRewardTime >= TimeSpan.FromMinutes(10),
            player =>
            {
                lastRewardTime = DateTime.UtcNow; // Update the timestamp
                player.AddToBackpack(new MaxxiaScroll()); // Give the reward
                player.SendGump(new DialogueGump(player, new DialogueModule("This reward is a symbol of our tribe's gratitude. May it serve you well on your journeys.")));
            });

        greeting.AddOption("Can you share a story?",
            player => true,
            player =>
            {
                DialogueModule storyModule = new DialogueModule("The stories of old teach us about courage, love, betrayal, and redemption. Do you want to hear a specific story?");
                storyModule.AddOption("Tell me about a great battle.",
                    pl => true,
                    pl => { pl.SendGump(new DialogueGump(pl, new DialogueModule("In the Battle of the Red Sky, our tribe united to face a great enemy. The sky turned crimson, and the spirits guided our hands."))); });
                storyModule.AddOption("Tell me a love story.",
                    pl => true,
                    pl => { pl.SendGump(new DialogueGump(pl, new DialogueModule("There once was a brave warrior who loved a shaman. Their love brought peace to the tribes and united them against common foes."))); });
                storyModule.AddOption("Tell me about betrayal.",
                    pl => true,
                    pl => { pl.SendGump(new DialogueGump(pl, new DialogueModule("A trusted warrior once betrayed us for greed. His actions taught us the importance of loyalty and the pain of betrayal."))); });
                player.SendGump(new DialogueGump(player, storyModule));
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
