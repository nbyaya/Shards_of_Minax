using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

[CorpseName("the corpse of Socrates the Questioner")]
public class SocratesTheQuestioner : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public SocratesTheQuestioner() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Socrates the Questioner";
        Body = 0x190; // Human male body

        // Stats
        SetStr(68);
        SetDex(52);
        SetInt(123);
        SetHits(59);

        // Appearance
        AddItem(new Robe() { Hue = 2209 });
        AddItem(new Sandals() { Hue = 1177 });
        AddItem(new Spellbook() { Name = "Socratic Dialogues" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

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
        DialogueModule greeting = new DialogueModule("Greetings, seeker of wisdom. I am Socrates the Questioner. What brings you here?");

        greeting.AddOption("Tell me about your name.",
            player => true,
            player => player.SendGump(new DialogueGump(player, new DialogueModule("I am known as Socrates the Questioner, a wanderer of thought. A name carries weight, but what do you believe defines a person?"))));

        greeting.AddOption("How is your health?",
            player => true,
            player => player.SendGump(new DialogueGump(player, new DialogueModule("My health is a reflection of my philosophical convictions. But tell me, what do you consider the true measure of health?"))));

        greeting.AddOption("What is your job?",
            player => true,
            player => player.SendGump(new DialogueGump(player, new DialogueModule("I am a seeker of truth. What is it that you seek on your journey? Is it knowledge, power, or perhaps something deeper?"))));

        greeting.AddOption("Tell me about battles.",
            player => true,
            player =>
            {
                DialogueModule battlesModule = new DialogueModule("Life is a series of battles fought not with weapons, but with questions. What do you consider the most important question in your life?");
                battlesModule.AddOption("What do you mean?",
                    p => true,
                    p => p.SendGump(new DialogueGump(p, new DialogueModule("The pursuit of wisdom is noble. Each question leads us further down the path of understanding."))));
                battlesModule.AddOption("I seek power above all.",
                    p => true,
                    p => {
                        DialogueModule powerModule = new DialogueModule("Power can be an illusion. What will you do with this power? Will it bring you peace or further conflict?");
                        powerModule.AddOption("I will use it to help others.",
                            pl => true,
                            pl => pl.SendGump(new DialogueGump(pl, new DialogueModule("A noble aim! But tell me, how will you define 'help'? Is it the same for all?"))));
                        powerModule.AddOption("I seek only my own gain.",
                            pl => true,
                            pl => pl.SendGump(new DialogueGump(pl, new DialogueModule("Ah, a pursuit of self! But does self-interest lead to true fulfillment? Or does it breed loneliness?"))));
                        p.SendGump(new DialogueGump(p, powerModule));
                    });
                player.SendGump(new DialogueGump(player, battlesModule));
            });

        greeting.AddOption("What is wisdom?",
            player => true,
            player => {
                DialogueModule wisdomModule = new DialogueModule("Wisdom transcends knowledge; it is the application of understanding. How do you distinguish between knowledge and wisdom in your life?");
                wisdomModule.AddOption("Knowledge is facts; wisdom is understanding.",
                    pl => true,
                    pl => pl.SendGump(new DialogueGump(pl, new DialogueModule("A fine distinction! But can knowledge exist without wisdom? Or vice versa?"))));
                wisdomModule.AddOption("I cannot tell the difference.",
                    pl => true,
                    pl => {
                        DialogueModule reflectionModule = new DialogueModule("Then perhaps you are on a journey to find that answer. What experiences have shaped your view of the world?");
                        reflectionModule.AddOption("My travels have taught me much.",
                            p => true,
                            p => p.SendGump(new DialogueGump(p, new DialogueModule("Travel broadens the mind. What lessons have you learned that will guide you forward?"))));
                        reflectionModule.AddOption("I have not traveled far.",
                            p => true,
                            p => p.SendGump(new DialogueGump(p, new DialogueModule("Ah, yet every moment holds wisdom if you seek it. What have you learned from your daily life?"))));
                        pl.SendGump(new DialogueGump(pl, reflectionModule));
                    });
                player.SendGump(new DialogueGump(player, wisdomModule));
            });

        greeting.AddOption("Can I have a token?",
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
                    player.SendGump(new DialogueGump(player, new DialogueModule("Here, take this small pendant. May it remind you of our conversations and the importance of seeking wisdom.")));
                    player.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            });

        greeting.AddOption("What is the meaning of life?",
            player => true,
            player =>
            {
                DialogueModule meaningModule = new DialogueModule("Ah, the age-old question! What do you believe is the purpose of our existence?");
                meaningModule.AddOption("To find happiness.",
                    pl => true,
                    pl => {
                        DialogueModule happinessModule = new DialogueModule("Happiness is a worthy pursuit! But is it a destination or a journey? How do you cultivate happiness in your life?");
                        happinessModule.AddOption("Through relationships.",
                            p => true,
                            p => p.SendGump(new DialogueGump(p, new DialogueModule("Indeed, connections with others can bring joy. Do you believe these relationships shape who you are?"))));
                        happinessModule.AddOption("Through achievements.",
                            p => true,
                            p => p.SendGump(new DialogueGump(p, new DialogueModule("Achievements can be fulfilling, yet can they provide lasting happiness? What happens when goals are met?"))));
                        pl.SendGump(new DialogueGump(pl, happinessModule));
                    });
                meaningModule.AddOption("To gain knowledge.",
                    pl => true,
                    pl => pl.SendGump(new DialogueGump(pl, new DialogueModule("Knowledge is powerful. But how do you measure the value of knowledge? Is it in its application or the pursuit?"))));
                meaningModule.AddOption("There is no meaning.",
                    pl => true,
                    pl => {
                        DialogueModule nihilismModule = new DialogueModule("A challenging perspective! If there is no meaning, what then drives your actions? Is it instinct, curiosity, or something else?");
                        nihilismModule.AddOption("It is instinct.",
                            p => true,
                            p => p.SendGump(new DialogueGump(p, new DialogueModule("Instinct can guide us, yet can it lead to fulfillment? What about conscious choices?"))));
                        nihilismModule.AddOption("It is curiosity.",
                            p => true,
                            p => p.SendGump(new DialogueGump(p, new DialogueModule("Curiosity can lead to discovery! What do you hope to uncover in your journey?"))));
                        pl.SendGump(new DialogueGump(pl, nihilismModule));
                    });
                player.SendGump(new DialogueGump(player, meaningModule));
            });

        return greeting;
    }

    public SocratesTheQuestioner(Serial serial) : base(serial) { }

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
