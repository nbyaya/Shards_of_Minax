using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a phantom assassin")]
    public class PhantomAssassin : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(10.0); // time between assassin speech
        public DateTime m_NextSpeechTime;

        [Constructable]
        public PhantomAssassin() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = 0x4001; // Dark hue for a phantom look
			Team = 1;

            Body = 0x190; // Human body
            Name = "Phantom Assassin";
            Title = "the Silent Killer";

            Item hood = new HoodedShroudOfShadows();
            Item weapon = new Dagger();

            hood.Hue = 0x455; // Dark hue
            weapon.Hue = 0x455; // Dark hue
            weapon.Layer = Layer.OneHanded;

            AddItem(hood);
            AddItem(weapon);
            weapon.Movable = false;

            SetStr(500, 700);
            SetDex(200, 300);
            SetInt(100, 200);

            SetHits(400, 600);

            SetDamage(15, 25);

            SetDamageType(ResistanceType.Physical, 75);
            SetDamageType(ResistanceType.Poison, 25);

            SetResistance(ResistanceType.Physical, 50, 65);
            SetResistance(ResistanceType.Fire, 30, 45);
            SetResistance(ResistanceType.Cold, 30, 45);
            SetResistance(ResistanceType.Poison, 60, 75);
            SetResistance(ResistanceType.Energy, 30, 45);

            SetSkill(SkillName.Anatomy, 80.1, 100.0);
            SetSkill(SkillName.EvalInt, 80.1, 100.0);
            SetSkill(SkillName.Magery, 90.5, 100.0);
            SetSkill(SkillName.Meditation, 75.1, 100.0);
            SetSkill(SkillName.MagicResist, 95.5, 100.0);
            SetSkill(SkillName.Tactics, 100.1, 120.0);
            SetSkill(SkillName.Wrestling, 100.1, 120.0);

            Fame = 7000;
            Karma = -7000;

            VirtualArmor = 40;

            m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
        }

        public override bool AlwaysMurderer { get { return true; } }
        public override bool CanRummageCorpses { get { return true; } }
        public override bool ShowFameTitle { get { return false; } }
        public override bool ClickTitle { get { return true; } }

        public override void OnThink()
        {
            if (DateTime.Now >= m_NextSpeechTime)
            {
                Mobile combatant = this.Combatant as Mobile;

                if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 8))
                {
                    int phrase = Utility.Random(4);

                    switch (phrase)
                    {
                        case 0: this.Say(true, "You won't see me coming!"); break;
                        case 1: this.Say(true, "I strike from the shadows."); break;
                        case 2: this.Say(true, "Your end is near."); break;
                        case 3: this.Say(true, "Silence is my ally."); break;
                    }

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }

                base.OnThink();
            }
        }

        public override void GenerateLoot()
        {
            PackGem();
            PackGold(300, 400);
            AddLoot(LootPack.Rich);

            int phrase = Utility.Random(2);

            switch (phrase)
            {
                case 0: this.Say(true, "The shadows consume me..."); break;
                case 1: this.Say(true, "My task is unfinished..."); break;
            }

            PackItem(new Nightshade(Utility.RandomMinMax(10, 20)));
        }

        public override int Damage(int amount, Mobile from)
        {
            Mobile combatant = this.Combatant as Mobile;

            if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 8))
            {
                if (Utility.RandomBool())
                {
                    int phrase = Utility.Random(4);

                    switch (phrase)
                    {
                        case 0: this.Say(true, "You cannot harm the shadows!"); break;
                        case 1: this.Say(true, "Is that the best you can do?"); break;
                        case 2: this.Say(true, "I am but a whisper in the night."); break;
                        case 3: this.Say(true, "You are already dead."); break;
                    }

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }
            }

            return base.Damage(amount, from);
        }

        public PhantomAssassin(Serial serial) : base(serial)
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
