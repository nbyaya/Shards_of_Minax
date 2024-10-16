using System;
using Server;
using Server.Mobiles;
using Server.Items;

[CorpseName("the corpse of Michael Psellus")]
public class MichaelPsellus : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public MichaelPsellus() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Michael Psellus";
        Body = 0x190; // Human male body
        SetStr(100);
        SetDex(60);
        SetInt(80);
        SetHits(70);

        AddItem(new Robe() { Hue = 1157 });
        AddItem(new Sandals() { Hue = 1157 });
        AddItem(new Spellbook() { Name = "Michael Psellus's Chronicle" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();
        SpeechHue = 0;

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
        DialogueModule greeting = new DialogueModule("Ah, you wish to converse with me, traveler?");

        greeting.AddOption("Tell me your name.",
            player => true,
            player => 
            {
                greeting.NPCText = "I am Michael Psellus, a humble scholar lost in the pages of history.";
                AddFollowUpOptions(greeting);
            });

        return greeting;
    }

    private void AddFollowUpOptions(DialogueModule module)
    {
        module.AddOption("What do you know about Byzantium?",
            player => true,
            player =>
            {
                DialogueModule byzantiumModule = new DialogueModule("Byzantium, a place of grandeur, where secrets linger in the shadows. Its tales are many, some forgotten, some buried in time.");
                byzantiumModule.AddOption("What secrets?",
                    playerq => true,
                    playerq =>
                    {
                        DialogueModule secretsModule = new DialogueModule("Ah, the secrets! They are whispered among the stones. Some say there is a hidden chamber, untouched for centuries.");
                        secretsModule.AddOption("Where can I find this chamber?",
                            playerw => true,
                            playerw =>
                            {
                                secretsModule.NPCText = "The entrance lies where moonlight kisses the ancient stones. Seek wisely, for it is guarded by shadows.";
                                player.SendGump(new DialogueGump(player, secretsModule));
                            });
                        secretsModule.AddOption("What treasures are hidden?",
                            playere => true,
                            playere =>
                            {
                                secretsModule.NPCText = "Legends speak of relics that can alter fate itself. But they are not easily won. Are you prepared to face danger?";
                                player.SendGump(new DialogueGump(player, secretsModule));
                            });
                        player.SendGump(new DialogueGump(player, secretsModule));
                    });

                byzantiumModule.AddOption("Tell me about its history.",
                    playerr => true,
                    playerr =>
                    {
                        byzantiumModule.NPCText = "Byzantium's history is a tapestry woven with the threads of time—an empire risen and fallen, stories waiting to be uncovered.";
                        byzantiumModule.AddOption("What tales do you know?",
                            playert => true,
                            playert =>
                            {
                                byzantiumModule.NPCText = "Tales of valor, betrayal, and intrigue. Many have sought glory, yet only a few have returned with their tales intact.";
                                player.SendGump(new DialogueGump(player, byzantiumModule));
                            });
                        player.SendGump(new DialogueGump(player, byzantiumModule));
                    });

                module.AddOption("Interesting. What of the battles fought here?",
                    playery => true,
                    playery =>
                    {
                        DialogueModule battlesModule = new DialogueModule("The battles of old echo in the streets. Many fought for power, others for survival. Each clash wrote a new chapter in our history.");
                        battlesModule.AddOption("Were there great heroes?",
                            playeru => true,
                            playeru =>
                            {
                                battlesModule.NPCText = "Indeed, heroes rose from the ashes of conflict. They were both revered and reviled. Their stories are etched in the annals of time.";
                                battlesModule.AddOption("What about their legacy?",
                                    playeri => true,
                                    playeri =>
                                    {
                                        battlesModule.NPCText = "Their legacies serve as both inspiration and warning. For every hero, a villain lurked, waiting to exploit their downfall.";
                                        player.SendGump(new DialogueGump(player, battlesModule));
                                    });
                                player.SendGump(new DialogueGump(player, battlesModule));
                            });

                        player.SendGump(new DialogueGump(player, battlesModule));
                    });

                player.SendGump(new DialogueGump(player, byzantiumModule));
            });

        module.AddOption("What can you tell me about relics?",
            player => true,
            player =>
            {
                DialogueModule relicsModule = new DialogueModule("Ah, relics! They are fragments of the past, imbued with magic and history. I possess a few myself, treasures of forgotten times.");
                relicsModule.AddOption("May I see them?",
                    playero => true,
                    playero =>
                    {
                        relicsModule.NPCText = "Ah, you wish to gaze upon them? But first, prove your worth. Complete a task, and I shall reveal them.";
                        relicsModule.AddOption("What task must I complete?",
                            playerp => true,
                            playerp =>
                            {
                                relicsModule.NPCText = "Seek the Golden Chalice hidden in the city. Bring it to me, and the relics shall be yours to behold.";
                                player.SendGump(new DialogueGump(player, relicsModule));
                            });
                        player.SendGump(new DialogueGump(player, relicsModule));
                    });

                module.AddOption("What treasures do you possess?",
                    playera => true,
                    playera =>
                    {
                        relicsModule.NPCText = "Each treasure has a story, a piece of history that speaks to those who listen. If you are curious, I might part with one for the right price.";
                        relicsModule.AddOption("What would you ask for a relic?",
                            players => true,
                            players =>
                            {
                                relicsModule.NPCText = "A task well done or a rare item. The value lies not only in gold but in the stories that accompany it.";
                                player.SendGump(new DialogueGump(player, relicsModule));
                            });
                        player.SendGump(new DialogueGump(player, relicsModule));
                    });

                player.SendGump(new DialogueGump(player, relicsModule));
            });

        module.AddOption("What can you teach me?",
            player => true,
            player =>
            {
                DialogueModule teachModule = new DialogueModule("Ah, a seeker of knowledge! What would you like to learn about? Alchemy, history, or perhaps the art of survival?");
                teachModule.AddOption("Tell me about alchemy.",
                    playerd => true,
                    playerd =>
                    {
                        teachModule.NPCText = "Alchemy is both art and science. It requires knowledge, precision, and a touch of intuition. Would you like to learn about potions or transmutation?";
                        teachModule.AddOption("Potions.",
                            playerf => true,
                            playerf =>
                            {
                                teachModule.NPCText = "Potions are created by combining ingredients in specific ratios. Each ingredient has unique properties that can produce various effects.";
                                teachModule.AddOption("What ingredients are rare?",
                                    playerg => true,
                                    playerg =>
                                    {
                                        teachModule.NPCText = "Some of the rarest ingredients include Dragon's Breath and Essence of the Phoenix. They are often guarded by fierce creatures.";
                                        player.SendGump(new DialogueGump(player, teachModule));
                                    });
                                player.SendGump(new DialogueGump(player, teachModule));
                            });

                        teachModule.AddOption("Transmutation.",
                            playerh => true,
                            playerh =>
                            {
                                teachModule.NPCText = "Transmutation involves changing one substance into another. It's complex and requires great skill. Are you ready to take on that challenge?";
                                player.SendGump(new DialogueGump(player, teachModule));
                            });

                        player.SendGump(new DialogueGump(player, teachModule));
                    });

                player.SendGump(new DialogueGump(player, teachModule));
            });
    }

    public MichaelPsellus(Serial serial) : base(serial) { }

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
