using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("corpse of R.J. Neuman, The Founder")]
    public class RJNeuman : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(4.0);
        public DateTime m_NextSpeechTime;

        [Constructable]
        public RJNeuman()
            : base(AIType.AI_Mage, FightMode.Closest, 8, 1, 0.2, 0.3)
        {
            Name = "R.J. Neuman";
            Title = "The Founder - OT0";

            Body = 0x190;
            Hue = 0; // Human skin tone, a bit pale

            SpeechHue = 1150;

            // Outfit – cheap but flashy
            AddItem(new FancyShirt() { Hue = 1153, Name = "Founder’s Raiment" });
            AddItem(new LongPants() { Hue = 1153 });
            AddItem(new Cloak() { Hue = 1175, Name = "Mantle of Ascendancy" });
            AddItem(new Boots() { Hue = 1109 });

            // Weapon – nothing but a fancy staff
            AddItem(new GoldBracelet() { Name = "Investor’s Cufflink" });
            AddItem(new GoldRing() { Name = "Signet of the First Investment" });

            // Stats – human level. He’s not a god.
            SetStr(80, 100);
            SetDex(90, 110);
            SetInt(120, 140);

            SetHits(300, 400);
            SetDamage(8, 12);

            SetSkill(SkillName.MagicResist, 60.0, 80.0);
            SetSkill(SkillName.Magery, 80.0, 90.0);
            SetSkill(SkillName.EvalInt, 80.0, 90.0);
            SetSkill(SkillName.Meditation, 60.0, 80.0);

            Fame = 2500; // Laughable compared to his "disciples"
            Karma = -2500;

            VirtualArmor = 20;

            m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
        }

        public override bool AlwaysMurderer { get { return false; } }
        public override bool CanRummageCorpses { get { return false; } }
        public override bool ClickTitle { get { return true; } }

        public override void OnThink()
        {
            base.OnThink();

            if (DateTime.Now >= m_NextSpeechTime)
            {
                Mobile combatant = this.Combatant as Mobile;

                if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 10))
                {
                    int phrase = Utility.Random(7);

                    switch (phrase)
                    {
                        case 0: Say("Stay calm! The system works if you just believe!"); break;
                        case 1: Say("This is all... *intentional*. The Broken Star has a plan."); break;
                        case 2: Say("You’re not seeing the *big picture*!"); break;
                        case 3: Say("My *vision* is just... ahead of its time."); break;
                        case 4: Say("Remember: all investments carry risk."); break;
                        case 5: Say("*quietly to himself* I can still salvage this."); break;
                        case 6: Say("*nervously* Ascend... ascend and pay your dues."); break;
                    }

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }
            }
        }

        public override int Damage(int amount, Mobile from)
        {
            if (from != null && from.Map == this.Map && from.InRange(this, 8))
            {
                Say("*winces* Violence violates the service agreement!");
            }

            return base.Damage(amount, from);
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (Utility.RandomDouble() < 0.3)
            {
                Say("*flails ineffectually* Obey your founder!");
            }
        }

        public override void OnGotMeleeAttack(Mobile attacker)
        {
            base.OnGotMeleeAttack(attacker);

            if (Utility.RandomDouble() < 0.2)
            {
                Say("*gasps* You can’t *fire* me! I’m the founder!");
            }
        }

        public override void OnDeath(Container c)
        {
            Say("*coughs* It was just... a startup... You don’t understand...");
            base.OnDeath(c);
        }

        public override void GenerateLoot()
        {
            PackGold(5000, 8000);
			PackItem(new FoundersLedger());
			PackItem(new BrokenStarPendantBlueprint());

            if (Utility.RandomDouble() < 0.2)
                PackItem(new GoldBracelet() { Name = "Founder’s Cufflink" });

            if (Utility.RandomDouble() < 0.1)
                PackItem(new GoldRing() { Name = "Investor’s Seal" });

            if (Utility.RandomDouble() < 0.05)
                PackItem(new GoldNecklace() { Name = "R.J. Neuman’s 'Broken Star' Pendant" });
        }

        public RJNeuman(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
