using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class BewitchingBellad : BaseCreature
{
    [Constructable]
    public BewitchingBellad() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Bewitching Bellad";
        Body = 0x191; // Human female body

        // Stats
        SetStr(87);
        SetDex(74);
        SetInt(54);
        SetHits(62);

        // Appearance
        AddItem(new FancyDress(2971)); // Clothing item with hue 2971
        AddItem(new Boots(2972)); // Boots with hue 2972

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();
    }

    public BewitchingBellad(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, dear. I am Bewitching Bellad. What brings you to me?");

        greeting.AddOption("Tell me about yourself.",
            player => true,
            player =>
            {
                DialogueModule aboutModule = new DialogueModule("Why do you care about my life, darling? No one usually does. I entertain those who can afford it, but what does it matter to you?");
                aboutModule.AddOption("I want to know your story.",
                    p => true,
                    p =>
                    {
                        DialogueModule storyModule = new DialogueModule("Oh, you want to know more about my life? Tell me, do you judge me for it?");
                        storyModule.AddOption("No, I do not judge you.",
                            pl => true,
                            pl =>
                            {
                                DialogueModule noJudgeModule = new DialogueModule("Many people judge, but very few truly understand. Thank you for your kindness. Amidst all the judgments, it's the untold stories that weigh me down.");
                                noJudgeModule.AddOption("Tell me one of those stories.",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule taleModule = new DialogueModule("Some tales tell of my exploits, others of my heartaches. But few know of my addiction to moonflowers. They consume my thoughts, my dreams, my very soul...");
                                        taleModule.AddOption("Moonflowers? What are they?",
                                            plq => true,
                                            plq =>
                                            {
                                                DialogueModule moonflowerModule = new DialogueModule("Ah, moonflowers... They bloom only under the light of a full moon, their petals glowing with an otherworldly hue. They're beautiful, yes, but they are also dangerous. They draw you in, whispering promises of euphoria, of escape. I can't resist them.");
                                                moonflowerModule.AddOption("Why are they dangerous?",
                                                    pll => true,
                                                    pll =>
                                                    {
                                                        DialogueModule dangerModule = new DialogueModule("They are dangerous because they take everything from you. They start by giving you a sense of peace, of release from all your troubles. But soon, they take your sleep, your sanity, and finally, your will to live. Yet, I cannot stay away from them... I need them, even knowing the cost.");
                                                        dangerModule.AddOption("That sounds terrifying.",
                                                            plla => true,
                                                            plla =>
                                                            {
                                                                DialogueModule terrifyingModule = new DialogueModule("Terrifying, yes, but also... exhilarating. The fear, the thrill, the sensation of being on the edge of oblivion. It is madness, but it is my madness. I live for those fleeting moments when everything else disappears, and there is only the moonlight and the scent of the flowers.");
                                                                terrifyingModule.AddOption("Is there no way to break free?",
                                                                    pllb => true,
                                                                    pllb =>
                                                                    {
                                                                        DialogueModule breakFreeModule = new DialogueModule("Break free? Oh, I have tried. Many times. But the allure is too strong. Every time I see the moon rise, I feel its pull. I know I should stop, but I am weak. The flowers call to me, and I answer. Perhaps one day, they will take me entirely, and then I will be free.");
                                                                        breakFreeModule.AddOption("I hope you find the strength to overcome it.",
                                                                            pllc => true,
                                                                            pllc =>
                                                                            {
                                                                                pllc.SendGump(new DialogueGump(pllc, CreateGreetingModule()));
                                                                            });
                                                                        breakFreeModule.AddOption("I don't think I can help you.",
                                                                            pllc => true,
                                                                            pllc =>
                                                                            {
                                                                                pllc.SendGump(new DialogueGump(pllc, CreateGreetingModule()));
                                                                            });
                                                                        pllb.SendGump(new DialogueGump(pllb, breakFreeModule));
                                                                    });
                                                                terrifyingModule.AddOption("I understand the allure.",
                                                                    pllb => true,
                                                                    pllb =>
                                                                    {
                                                                        pllb.SendGump(new DialogueGump(pllb, CreateGreetingModule()));
                                                                    });
                                                                plla.SendGump(new DialogueGump(plla, terrifyingModule));
                                                            });
                                                        dangerModule.AddOption("You must stop before it's too late.",
                                                            plla => true,
                                                            plla =>
                                                            {
                                                                DialogueModule stopModule = new DialogueModule("Too late? It was too late the moment I first tasted the nectar of those cursed blooms. They had me then, and they have me still. But perhaps... perhaps you are right. Maybe there is still a sliver of hope, somewhere deep inside.");
                                                                stopModule.AddOption("Hold on to that hope.",
                                                                    pllb => true,
                                                                    pllb =>
                                                                    {
                                                                        pllb.SendGump(new DialogueGump(pllb, CreateGreetingModule()));
                                                                    });
                                                                stopModule.AddOption("I doubt you will ever escape.",
                                                                    pllb => true,
                                                                    pllb =>
                                                                    {
                                                                        pllb.SendGump(new DialogueGump(pllb, CreateGreetingModule()));
                                                                    });
                                                                plla.SendGump(new DialogueGump(plla, stopModule));
                                                            });
                                                        pll.SendGump(new DialogueGump(pll, dangerModule));
                                                    });
                                                moonflowerModule.AddOption("Where do you find these moonflowers?",
                                                    pll => true,
                                                    pll =>
                                                    {
                                                        DialogueModule locationModule = new DialogueModule("They grow deep in the Whispering Woods, in a clearing bathed in moonlight. It is a place of beauty, but also of great sorrow. Many have ventured there, seeking the moonflowers' embrace, and few have returned unchanged. If you go there, be careful, for the flowers will whisper to you, just as they do to me.");
                                                        locationModule.AddOption("I will avoid that place.",
                                                            plla => true,
                                                            plla =>
                                                            {
                                                                plla.SendGump(new DialogueGump(plla, CreateGreetingModule()));
                                                            });
                                                        locationModule.AddOption("Perhaps I will visit it.",
                                                            plla => true,
                                                            plla =>
                                                            {
                                                                plla.SendGump(new DialogueGump(plla, CreateGreetingModule()));
                                                            });
                                                        pll.SendGump(new DialogueGump(pll, locationModule));
                                                    });
                                                pl.SendGump(new DialogueGump(pl, moonflowerModule));
                                            });
                                        taleModule.AddOption("Maybe another time.",
                                            plw => true,
                                            plw =>
                                            {
                                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, taleModule));
                                    });
                                noJudgeModule.AddOption("Perhaps another time.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, noJudgeModule));
                            });
                        storyModule.AddOption("I think you should change your ways.",
                            pl => true,
                            pl =>
                            {
                                DialogueModule judgeModule = new DialogueModule("I've been judged by many, but it's not the judgments that weigh me down; it's the weight of the untold stories and forgotten mantras.");
                                judgeModule.AddOption("I understand.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, judgeModule));
                            });
                        p.SendGump(new DialogueGump(p, storyModule));
                    });
                aboutModule.AddOption("I didn't mean to pry.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, aboutModule));
            });

        greeting.AddOption("What do you think of society?",
            player => true,
            player =>
            {
                DialogueModule societyModule = new DialogueModule("It's amusing how society condemns me while secretly indulging in my services. Do you agree?");
                societyModule.AddOption("I agree, it is hypocritical.",
                    p => true,
                    p =>
                    {
                        DialogueModule agreeModule = new DialogueModule("It's true. They shun me by day but seek me by night. Amidst the shadows and secrets, I once overheard that the third syllable of the mantra of Spirituality is LOR.");
                        agreeModule.AddOption("Thank you for sharing.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, agreeModule));
                    });
                societyModule.AddOption("I don't think it's right.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, societyModule));
            });

        greeting.AddOption("Do you have any secrets to share?",
            player => true,
            player =>
            {
                DialogueModule secretsModule = new DialogueModule("In the hushed corners of taverns, and in the embraces of the night, secrets are whispered. Some light as air, others heavy as stone. What kind of secret do you seek?");
                secretsModule.AddOption("Tell me a light secret.",
                    p => true,
                    p =>
                    {
                        p.SendMessage("Bewitching Bellad smiles enigmatically and says, 'The third syllable of the mantra of Spirituality is LOR.'");
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                secretsModule.AddOption("Tell me a heavy secret.",
                    p => true,
                    p =>
                    {
                        DialogueModule heavySecretModule = new DialogueModule("Many people in our world hide behind masks. Some to protect themselves, others to deceive. But in the end, masks always fall.");
                        heavySecretModule.AddOption("That's profound.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, heavySecretModule));
                    });
                player.SendGump(new DialogueGump(player, secretsModule));
            });

        greeting.AddOption("Goodbye.",
            player => true,
            player =>
            {
                player.SendMessage("Bewitching Bellad nods slowly, her eyes lingering on yours as you turn away.");
            });

        return greeting;
    }

    public override void Serialize(GenericWriter writer)
    {
        base.Serialize(writer);
        writer.Write(0); // version
    }

    public override void Deserialize(GenericReader reader)
    {
        base.Deserialize(reader);
        int version = reader.ReadInt();
    }
}