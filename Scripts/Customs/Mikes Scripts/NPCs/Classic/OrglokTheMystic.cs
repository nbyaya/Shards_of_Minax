using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

[CorpseName("the corpse of Orglok the Mystic")]
public class OrglokTheMystic : BaseCreature
{
    private DateTime lastInteractionTime;

    [Constructable]
    public OrglokTheMystic() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Orglok the Mystic";
        Body = 0x191; // Orc body

        // Stats
        SetStr(90);
        SetDex(70);
        SetInt(110);
        SetHits(80);

        // Appearance
        AddItem(new BoneArms() { Hue = 2212 });
        AddItem(new BoneLegs() { Hue = 2212 });
        AddItem(new BoneChest() { Hue = 2212 });
        AddItem(new BoneGloves() { Hue = 2212 });
        AddItem(new OrcMask() { Hue = 2213 });
        AddItem(new QuarterStaff() { Name = "Orglok's Staff" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        SpeechHue = 0; // Default speech hue

        // Initialize the lastInteractionTime to a past time
        lastInteractionTime = DateTime.MinValue;
    }

    public OrglokTheMystic(Serial serial) : base(serial) { }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule();
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule()
    {
        DialogueModule greeting = new DialogueModule("I am Orglok the Mystic, seeker of hidden truths. How may I assist you, traveler?");

        greeting.AddOption("What can you tell me about yourself?",
            player => true,
            player =>
            {
                DialogueModule nameModule = new DialogueModule("I commune with the spirits and seek the wisdom of the ancients. My life is dedicated to understanding the mysteries that bind our world to the divine.");
                player.SendGump(new DialogueGump(player, nameModule));
            });

        greeting.AddOption("How is your health?",
            player => true,
            player =>
            {
                DialogueModule healthModule = new DialogueModule("I am in harmony with the energies of this world, but harmony is a delicate balance. Tell me, do you seek the same?");
                player.SendGump(new DialogueGump(player, healthModule));
            });

        greeting.AddOption("What is your job?",
            player => true,
            player =>
            {
                DialogueModule jobModule = new DialogueModule("I commune with the spirits and seek the wisdom of the ancients. My duties involve guiding lost souls and interpreting the signs from beyond.");
                player.SendGump(new DialogueGump(player, jobModule));
            });

        greeting.AddOption("What is wisdom?",
            player => true,
            player =>
            {
                DialogueModule wisdomModule = new DialogueModule("The path to enlightenment is paved with humility and self-reflection. Art thou humble?");
                wisdomModule.AddOption("Yes, I am humble.",
                    p => CanShareWisdom(),
                    p =>
                    {
                        lastInteractionTime = DateTime.UtcNow; // Update the timestamp
                        DialogueModule shareWisdomModule = new DialogueModule("Then seek inner peace and balance, for the truth lies within. But tell me, what is it that you truly seek in this life?");
                        shareWisdomModule.AddOption("I seek understanding of the divine.",
                            pl => true,
                            pl =>
                            {
                                DialogueModule divineModule = new DialogueModule("Ah, the divine! It is a vast ocean of energy and light, often obscured by the fog of our doubts and fears. Have you ever felt a connection to something greater than yourself?");
                                divineModule.AddOption("Yes, I have experienced moments of clarity.",
                                    pq => true,
                                    pq =>
                                    {
                                        DialogueModule clarityModule = new DialogueModule("Such moments are gifts from the cosmos. They remind us of our true nature. In those fleeting instances, we touch the divine. How do you interpret those experiences?");
                                        clarityModule.AddOption("I believe they are messages from the universe.",
                                            plw => true,
                                            plw =>
                                            {
                                                DialogueModule messageModule = new DialogueModule("Indeed! The universe speaks in whispers and signs, guiding those who are willing to listen. Every synchronicity, every chance encounter, is a thread woven into the tapestry of fate.");
                                                messageModule.AddOption("What if I miss the signs?",
                                                    pla => true,
                                                    pla =>
                                                    {
                                                        DialogueModule missModule = new DialogueModule("Fear not! The universe has a way of bringing you back on course. Be patient, and stay attuned to your surroundings. Trust your intuition; it is the voice of the divine.");
                                                        pla.SendGump(new DialogueGump(pla, missModule));
                                                    });
                                                pl.SendGump(new DialogueGump(pl, messageModule));
                                            });
                                        clarityModule.AddOption("I see them as coincidences.",
                                            ple => true,
                                            ple =>
                                            {
                                                DialogueModule coincidenceModule = new DialogueModule("Ah, but what is coincidence but a name we give to the unknown? Perhaps the universe is more connected than we realize. What would you say to those who dismiss these experiences?");
                                                coincidenceModule.AddOption("They are blind to the truth.",
                                                    pla => true,
                                                    pla =>
                                                    {
                                                        DialogueModule blindModule = new DialogueModule("Indeed, their eyes are closed to the wonders that surround us. Encourage them to open their hearts and minds; the journey of discovery is worth every effort.");
                                                        pla.SendGump(new DialogueGump(pla, blindModule));
                                                    });
                                                coincidenceModule.AddOption("They are lost in their own ways.",
                                                    pla => true,
                                                    pla =>
                                                    {
                                                        DialogueModule lostModule = new DialogueModule("True, each person walks their own path. We must not judge but rather offer guidance to those who seek it.");
                                                        lostModule.AddOption("What if they refuse to listen?",
                                                            plar => true,
                                                            plar =>
                                                            {
                                                                DialogueModule refuseModule = new DialogueModule("Then we must be patient and continue to shine our own light. Sometimes, the best way to inspire others is to embody the truth ourselves.");
                                                                pla.SendGump(new DialogueGump(pla, refuseModule));
                                                            });
                                                        pla.SendGump(new DialogueGump(pla, lostModule));
                                                    });
                                                pl.SendGump(new DialogueGump(pl, coincidenceModule));
                                            });
                                        player.SendGump(new DialogueGump(player, clarityModule));
                                    });
                                divineModule.AddOption("No, I feel lost in the chaos.",
                                    pt => true,
                                    pt =>
                                    {
                                        DialogueModule lostInChaosModule = new DialogueModule("Ah, chaos is a natural part of life. Embrace it, for in chaos lies the potential for creation. Meditation and reflection can guide you back to clarity. Would you like to learn how?");
                                        lostInChaosModule.AddOption("Yes, please teach me!",
                                            ply => true,
                                            ply =>
                                            {
                                                DialogueModule teachModule = new DialogueModule("To connect with the divine, one must quiet the mind. Sit in stillness and breathe deeply. Focus on your breath and let your thoughts pass like clouds in the sky. Over time, you will find the peace you seek.");
                                                teachModule.AddOption("I will try this practice.",
                                                    pla => true,
                                                    pla =>
                                                    {
                                                        pla.SendMessage("Orglok smiles and nods approvingly.");
                                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                                    });
                                                pl.SendGump(new DialogueGump(pl, teachModule));
                                            });
                                        lostInChaosModule.AddOption("No, I prefer the chaos.",
                                            plu => true,
                                            plu =>
                                            {
                                                DialogueModule chaosModule = new DialogueModule("Very well! Just remember that amidst chaos, there is also opportunity. Seek the lessons that lie within the storms of life.");
                                                chaosModule.AddOption("Thank you for your insight.",
                                                    pla => true,
                                                    pla =>
                                                    {
                                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                                    });
                                                p.SendGump(new DialogueGump(p, chaosModule));
                                            });
                                        player.SendGump(new DialogueGump(player, lostInChaosModule));
                                    });
                                pl.SendGump(new DialogueGump(pl, divineModule));
                            });
                        shareWisdomModule.AddOption("I seek enlightenment.",
                            pl => true,
                            pl =>
                            {
                                DialogueModule enlightenmentModule = new DialogueModule("Ah, enlightenment is a journey, not a destination. It requires dedication and perseverance. Have you embarked on any practices to aid your journey?");
                                enlightenmentModule.AddOption("I meditate daily.",
                                    pi => true,
                                    pi =>
                                    {
                                        DialogueModule meditateModule = new DialogueModule("Meditation is a powerful tool. It opens the gateway to deeper understanding. As you sit in silence, allow the stillness to wash over you. What do you hope to achieve through your meditation?");
                                        meditateModule.AddOption("I seek inner peace.",
                                            plo => true,
                                            plo =>
                                            {
                                                DialogueModule peaceModule = new DialogueModule("Inner peace is a noble pursuit. As you cultivate it, let go of your attachments to the external world. The more you practice, the more peace you will find.");
                                                peaceModule.AddOption("Thank you for the guidance.",
                                                    pla => true,
                                                    pla =>
                                                    {
                                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                                    });
                                                p.SendGump(new DialogueGump(p, peaceModule));
                                            });
                                        meditateModule.AddOption("I want to connect with the divine.",
                                            plp => true,
                                            plp =>
                                            {
                                                DialogueModule divineConnectionModule = new DialogueModule("To connect with the divine, focus your heart as much as your mind. Visualize the light within you merging with the greater light of the universe. Feel the connection strengthen with each breath.");
                                                divineConnectionModule.AddOption("I will practice this connection.",
                                                    pla => true,
                                                    pla =>
                                                    {
                                                        pla.SendMessage("Orglok smiles approvingly.");
                                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                                    });
                                                p.SendGump(new DialogueGump(p, divineConnectionModule));
                                            });
                                        player.SendGump(new DialogueGump(player, meditateModule));
                                    });
                                enlightenmentModule.AddOption("I am uncertain about my path.",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule uncertainPathModule = new DialogueModule("Doubt is a natural part of any journey. Seek guidance from within and allow your intuition to lead you. Often, the answers we seek are buried deep in our hearts.");
                                        uncertainPathModule.AddOption("How do I find my intuition?",
                                            plax => true,
                                            plax =>
                                            {
                                                DialogueModule findIntuitionModule = new DialogueModule("Trust your instincts. When faced with decisions, pause and listen to that inner voice. Write down your feelings and thoughts; often, clarity arises from reflection.");
                                                findIntuitionModule.AddOption("I will start journaling my thoughts.",
                                                    ps => true,
                                                    ps =>
                                                    {
                                                        p.SendMessage("Orglok nods approvingly.");
                                                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                                                    });
                                                pla.SendGump(new DialogueGump(pla, findIntuitionModule));
                                            });
                                        player.SendGump(new DialogueGump(player, uncertainPathModule));
                                    });
                                player.SendGump(new DialogueGump(player, enlightenmentModule));
                            });
                        player.SendGump(new DialogueGump(player, shareWisdomModule));
                    });
                wisdomModule.AddOption("No, I am not humble.",
                    p => true,
                    p =>
                    {
                        DialogueModule notHumbleModule = new DialogueModule("Ah, pride can be a barrier to wisdom. It clouds the heart and dulls the spirit. How do you feel about your current state of being?");
                        notHumbleModule.AddOption("I feel confident in my abilities.",
                            pl => true,
                            pl =>
                            {
                                DialogueModule confidenceModule = new DialogueModule("Confidence can be a powerful ally, but beware of the fine line between confidence and arrogance. Reflect upon your motivations. Do they serve you and others?");
                                confidenceModule.AddOption("I seek to be a leader.",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule leaderModule = new DialogueModule("A noble pursuit! A true leader guides with compassion and wisdom. Remember, leadership is not about power, but about uplifting those around you.");
                                        pla.SendGump(new DialogueGump(pla, leaderModule));
                                    });
                                confidenceModule.AddOption("I am only looking out for myself.",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule selfishModule = new DialogueModule("A path of selfishness leads to isolation. True fulfillment comes from connections with others and the shared journey toward enlightenment. Reflect on how you can be of service.");
                                        pla.SendGump(new DialogueGump(pla, selfishModule));
                                    });
                                p.SendGump(new DialogueGump(p, confidenceModule));
                            });
                        notHumbleModule.AddOption("I seek wealth and power.",
                            pl => true,
                            pl =>
                            {
                                DialogueModule wealthModule = new DialogueModule("Wealth and power can be enticing, but they are fleeting. True richness lies in experiences and connections. Consider how you can use your resources to create lasting impact.");
                                wealthModule.AddOption("I will think about this.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                p.SendGump(new DialogueGump(p, wealthModule));
                            });
                        player.SendGump(new DialogueGump(player, notHumbleModule));
                    });
                player.SendGump(new DialogueGump(player, wisdomModule));
            });

        return greeting;
    }

    private bool CanShareWisdom()
    {
        TimeSpan cooldown = TimeSpan.FromMinutes(10);
        if (DateTime.UtcNow - lastInteractionTime < cooldown)
        {
            return false; // Wisdom can't be shared again yet
        }
        return true; // Wisdom can be shared
    }

    public override void Serialize(GenericWriter writer)
    {
        base.Serialize(writer);
        writer.Write((int)0); // version
        writer.Write(lastInteractionTime);
    }

    public override void Deserialize(GenericReader reader)
    {
        base.Deserialize(reader);
        int version = reader.ReadInt();
        lastInteractionTime = reader.ReadDateTime();
    }
}
