using System;
using Server;
using Server.Items;
using System.Collections;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("corpse of a corrupted miner")]
    public class MinerOfMinax : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(12.0);
        public DateTime m_NextSpeechTime;

        [Constructable]
        public MinerOfMinax() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.3, 0.5)
        {
            Name = "Miner of Minax";
            Title = "the Eternal Dredger";
            Body = 0x190;
            Hue = 1109; // Shadowy gray

            // Equipment
            AddItem(new Pickaxe());
            AddItem(new Cloak(1102)); // Dark Cloak
			AddItem(new HalfApron(Utility.RandomNeutralHue()));
			AddItem(new Cap(Utility.RandomNeutralHue()));
            AddItem(new LeatherGloves());
            AddItem(new Boots(Utility.RandomNeutralHue()));

            // Stats
            SetStr(450, 500);
            SetDex(150, 180);
            SetInt(60, 80);

            SetHits(150, 200);
            SetDamage(18, 26);

            SetSkill(SkillName.MagicResist, 70.0, 90.0);
            SetSkill(SkillName.Tactics, 75.0, 95.0);
            SetSkill(SkillName.Macing, 85.0, 100.0); // Uses pickaxe effectively

            Fame = 12000;
            Karma = -12000;

            VirtualArmor = 50;

            SpeechHue = 1153;

            m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
        }

        public override bool AlwaysMurderer { get { return true; } }
        public override bool CanRummageCorpses { get { return true; } }
        public override bool ClickTitle { get { return true; } }

        public override void OnThink()
        {
            base.OnThink();

            if (DateTime.Now >= m_NextSpeechTime)
            {
                Mobile combatant = this.Combatant as Mobile;

                if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 8))
                {
                    int phrase = Utility.Random(5);

                    switch (phrase)
                    {
                        case 0: this.Say("The mines whisper... *chittering sounds*"); break;
                        case 1: this.Say("Stone and shadow! You can't have the ore!"); break;
                        case 2: this.Say("Minax feeds... I serve eternally."); break;
                        case 3: this.Say("*taps pickaxe rhythmically against stone*"); break;
                        case 4: this.Say("You hear them too... don't you?"); break;
                    }

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }
            }
        }

        public override int Damage(int amount, Mobile from)
        {
            Mobile combatant = this.Combatant as Mobile;

            if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 8) && Utility.RandomBool())
            {
                int phrase = Utility.Random(4);

                switch (phrase)
                {
                    case 0: this.Say("Your strikes are dust."); break;
                    case 1: this.Say("*shrieks* The tunnels will consume you!"); break;
                    case 2: this.Say("Pain sharpens the mind..."); break;
                    case 3: this.Say("The ore bleeds with me!"); break;
                }

                m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
            }

            return base.Damage(amount, from);
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (Utility.RandomDouble() < 0.2)
            {
                this.Say("Crack and crumble! Fall before Minax!");
            }
        }

        public override void OnGotMeleeAttack(Mobile attacker)
        {
            base.OnGotMeleeAttack(attacker);

            if (Utility.RandomDouble() < 0.2)
            {
                this.Say("Your blows wake the things below...");
            }
        }

        public override void OnDeath(Container c)
        {
            this.Say("The mine... will never let me go...");
            base.OnDeath(c);
        }

        public override void GenerateLoot()
        {
            PackGold(250, 350);
            PackItem(new IronOre(Utility.RandomMinMax(5, 15)));

            if (Utility.RandomDouble() < 0.15)
                PackItem(new RelicFragment()); // Optional: rare loot


        }

        public MinerOfMinax(Serial serial) : base(serial)
        {
        }

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
