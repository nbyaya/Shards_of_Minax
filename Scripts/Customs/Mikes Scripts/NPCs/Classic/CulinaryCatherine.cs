using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Culinary Catherine")]
    public class CulinaryCatherine : BaseCreature
    {
        private DateTime m_NextRewardTime;

        [Constructable]
        public CulinaryCatherine() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Culinary Catherine";
            Body = 0x191; // Human female body

            // Stats
            Str = 75;
            Dex = 55;
            Int = 80;
            Hits = 55;

            // Appearance
            AddItem(new Kilt(2920));
            AddItem(new Surcoat(1151));
            AddItem(new Boots(495));
            AddItem(new LeatherGloves() { Name = "Catherine's Culinary Mitts" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            SpeechHue = 0; // Default speech hue
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!from.InRange(this, 3))
                return;

            if (Insensitive.Contains(e.Speech, "name"))
            {
                Say("Oh, it's you. What do you want?");
            }
            else if (Insensitive.Contains(e.Speech, "health"))
            {
                Say("Do I look like a healer to you? Figure it out yourself!");
            }
            else if (Insensitive.Contains(e.Speech, "job"))
            {
                Say("My job? I'm a cook, alright? Not that anyone appreciates it!");
            }
            else if (Insensitive.Contains(e.Speech, "cuisine"))
            {
                Say("Appreciation for culinary skills? Hah! Tell me, do you even know what 'sous-vide' means?");
            }
            else if (Insensitive.Contains(e.Speech, "riddles"))
            {
                Say("Oh, you think you're clever, don't you? Well, I don't have time for riddles. Scram!");
            }
            else if (Insensitive.Contains(e.Speech, "catherine"))
            {
                Say("Culinary Catherine, that's what they call me. Though most don't care enough to ask.");
            }
            else if (Insensitive.Contains(e.Speech, "healer"))
            {
                Say("Maybe if people took the time to enjoy my dishes, they'd feel better! Food is the best medicine after all.");
            }
            else if (Insensitive.Contains(e.Speech, "cook"))
            {
                Say("It's a thankless job. Day in and day out, I create masterpieces, and yet it's never enough. You know, I once prepared a feast for the King himself.");
            }
            else if (Insensitive.Contains(e.Speech, "care"))
            {
                if (DateTime.Now >= m_NextRewardTime)
                {
                    Say("It's a lonely world when you pour your heart into your work and no one notices. But here, I appreciate you asking. Take this as a token of my gratitude.");
                    from.AddToBackpack(new MaxxiaScroll()); // Reward item
                    m_NextRewardTime = DateTime.Now.AddMinutes(10); // 10 minute cooldown
                }
                else
                {
                    Say("You've already received a token of my appreciation recently. Come back later.");
                }
            }
            else if (Insensitive.Contains(e.Speech, "dishes"))
            {
                Say("My dishes aren't just food; they're art. From the flavors to the presentation, everything is meticulous. Have you ever tried my signature beef bourguignon?");
            }
            else if (Insensitive.Contains(e.Speech, "feast"))
            {
                Say("Ah, that was a day to remember. Fresh game, exotic fruits, and the finest wines. All for the royals and yet not a single compliment came my way.");
            }
            else if (Insensitive.Contains(e.Speech, "token"))
            {
                Say("It's not much, but it's a recipe I've been perfecting for ages. Use it well, and maybe you'll understand the essence of my culinary journey.");
            }
            else if (Insensitive.Contains(e.Speech, "bourguignon"))
            {
                Say("Ah, a connoisseur! That dish takes hours of slow cooking, with the finest cuts of beef, wine, and herbs. The secret? A dash of love and patience.");
            }
            else if (Insensitive.Contains(e.Speech, "royals"))
            {
                Say("Dealing with the royals is a mixed blessing. The prestige is unmatched, but the pressure is immense. And their tastes? Incredibly fickle.");
            }
            else if (Insensitive.Contains(e.Speech, "recipe"))
            {
                Say("This recipe has been passed down through generations. It's a simple dish, but when made right, it can transport you to another world.");
            }
            else if (Insensitive.Contains(e.Speech, "secret"))
            {
                Say("Every chef has their secrets. For some, it's a special ingredient. For others, a unique technique. For me? It's the passion I pour into every dish.");
            }
            else if (Insensitive.Contains(e.Speech, "pressure"))
            {
                Say("With every dish I present, I feel the weight of expectations. But it's that pressure that drives me to innovate and perfect my craft.");
            }
            else if (Insensitive.Contains(e.Speech, "generations"))
            {
                Say("My grandmother was a chef, as was her mother before her. Cooking is in my blood. It's a legacy I intend to uphold.");
            }
            else if (Insensitive.Contains(e.Speech, "passion"))
            {
                Say("Passion is what keeps me going. Even when the world is unkind, the fire in my heart never dims. It's the essence of every dish I create.");
            }
            else if (Insensitive.Contains(e.Speech, "innovate"))
            {
                Say("To innovate is to evolve. In the culinary world, if you aren't pushing boundaries, you're falling behind. But it's a balance between tradition and novelty.");
            }
            else if (Insensitive.Contains(e.Speech, "legacy"))
            {
                Say("Legacy is not just about remembering the past, but shaping the future. I hope my dishes leave an impact long after I'm gone.");
            }
            else if (Insensitive.Contains(e.Speech, "fire"))
            {
                Say("The fire represents my drive, my determination. It's what fuels my creativity and keeps me striving for perfection.");
            }
            else if (Insensitive.Contains(e.Speech, "tradition"))
            {
                Say("Tradition grounds us, gives us a sense of belonging. But it's also important to challenge and reinvent those traditions to stay relevant.");
            }
            else if (Insensitive.Contains(e.Speech, "impact"))
            {
                Say("The true impact of a dish is not in its taste alone, but in the memories it creates, the emotions it evokes. That's the mark of true culinary greatness.");
            }
            else if (Insensitive.Contains(e.Speech, "determination"))
            {
                Say("Determination is what pushes me through the long hours, the critiques, the failures. It's the backbone of any true artist.");
            }
            else if (Insensitive.Contains(e.Speech, "belonging"))
            {
                Say("The kitchen is where I belong. It's my sanctuary, my playground. It's where I'm truly myself.");
            }
            else if (Insensitive.Contains(e.Speech, "memories"))
            {
                Say("Memories are the soul's food. A single bite can take you back to a cherished moment, a long-forgotten place. It's magic, really.");
            }
            else if (Insensitive.Contains(e.Speech, "backbone"))
            {
                Say("Without a strong backbone, a chef would crumble under the pressure. It's what keeps us standing tall, despite the odds.");
            }
            else if (Insensitive.Contains(e.Speech, "sanctuary"))
            {
                Say("My sanctuary is my safe haven. A place where I can experiment, create, and be free from judgment. It's where my best ideas come to life.");
            }
            else if (Insensitive.Contains(e.Speech, "magic"))
            {
                Say("The magic of cooking is in the transformation. Taking simple ingredients and turning them into something extraordinary. It's a kind of alchemy.");
            }

            base.OnSpeech(e);
        }

        public CulinaryCatherine(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
