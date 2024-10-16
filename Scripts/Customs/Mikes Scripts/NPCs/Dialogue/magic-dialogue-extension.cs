using System;
using Server;
using Server.Mobiles;

public static class MagicDialogueExtension
{
    public static DialogueModule GetMagicModule(PlayerMobile player)
    {
        DialogueModule magicModule = new DialogueModule("Ah, the mystical arts! What aspect of magic intrigues you the most?");

        magicModule.AddOption("Tell me about spell circles.",
            p => true,
            p => {
                DialogueModule spellCirclesModule = new DialogueModule(
                    "Spell circles are a way of categorizing spells based on their power and complexity. " +
                    "There are eight circles, with the first being the simplest and the eighth being the most powerful. " +
                    "As you progress in your magical studies, you'll gain access to higher circles.");
                spellCirclesModule.AddOption("How do I learn new spell circles?",
                    pp => true,
                    pp => {
                        pp.SendMessage("To learn new spell circles, you must increase your Magery skill and often your Intelligence as well.");
                        pp.SendGump(new DialogueGump(pp, GetMagicModule(pp)));
                    });
                spellCirclesModule.AddOption("Return to magic topics",
                    pp => true,
                    pp => pp.SendGump(new DialogueGump(pp, GetMagicModule(pp))));
                p.SendGump(new DialogueGump(p, spellCirclesModule));
            });

        magicModule.AddOption("Explain the schools of magic.",
            p => true,
            p => {
                DialogueModule schoolsModule = new DialogueModule(
                    "There are several schools of magic, each focusing on different aspects of the arcane. " +
                    "Some major schools include Magery (general spellcasting), Necromancy (death magic), " +
                    "Chivalry (paladin spells), and Bushido (samurai techniques).");
                schoolsModule.AddOption("Which school should I study?",
                    pp => true,
                    pp => {
                        pp.SendMessage("The school you choose depends on your character's path. Magery is versatile, Necromancy is powerful but dark, Chivalry suits righteous warriors, and Bushido is for honorable fighters.");
                        pp.SendGump(new DialogueGump(pp, GetMagicModule(pp)));
                    });
                schoolsModule.AddOption("Return to magic topics",
                    pp => true,
                    pp => pp.SendGump(new DialogueGump(pp, GetMagicModule(pp))));
                p.SendGump(new DialogueGump(p, schoolsModule));
            });

        magicModule.AddOption("How do I improve my magical abilities?",
            p => true,
            p => {
                DialogueModule improveModule = new DialogueModule(
                    "Improving your magical abilities requires practice and study. Cast spells often, " +
                    "meditate to recover mana, and seek out magical tomes and scrolls. " +
                    "Remember, intelligence is key for any aspiring mage.");
                improveModule.AddOption("Any specific tips for beginners?",
                    pp => pp.Skills.Magery.Base < 50.0,
                    pp => {
                        pp.SendMessage("Focus on mastering the first and second circle spells before moving on. Practice Meditation to improve your mana regeneration.");
                        pp.SendGump(new DialogueGump(pp, GetMagicModule(pp)));
                    });
                improveModule.AddOption("Advanced magical training?",
                    pp => pp.Skills.Magery.Base >= 50.0,
                    pp => {
                        pp.SendMessage("Seek out rare magical artifacts and challenge yourself with difficult spells. Consider specializing in a particular school of magic.");
                        pp.SendGump(new DialogueGump(pp, GetMagicModule(pp)));
                    });
                improveModule.AddOption("Return to magic topics",
                    pp => true,
                    pp => pp.SendGump(new DialogueGump(pp, GetMagicModule(pp))));
                p.SendGump(new DialogueGump(p, improveModule));
            });

        magicModule.AddOption("I've heard enough about magic.",
            p => true,
            p => p.SendMessage("May your magical journey be filled with wonder and discovery!"));

        return magicModule;
    }
}
