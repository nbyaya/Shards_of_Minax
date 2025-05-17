using System;
using Server;
using Server.Items;
using System.Collections;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("corpse of a Mine Overseer")]
    public class MineOverseer : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(10.0);
        public DateTime m_NextSpeechTime;

        [Constructable]
        public MineOverseer()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.3, 0.5)
        {
			// Random names list
			string[] names = new string[]
			{
				"Vargul", "Drosk", "Karn", "Maldrek", "Suthar",
				"Graven", "Thorgil", "Vesk", "Drogan", "Balrek",
				"Kazgul", "Orm", "Brannoc", "Hask", "Varik"
			};

			// Titles for the Overseer
			string[] titles = new string[]
			{
				"the Taskmaster",
				"the Chainlord",
				"the Slave-Binder",
				"the Overseer of Sorrows",
				"the Pit Tyrant",
				"the Ironhand",
				"the Vein-Warden",
				"the Flesh Broker",
				"the Lash of Minax",
				"the Tormentor",
				"the Hollow-Eyed",
				"the Stoneheart"
			};

			// Assign random name and title
			Name = names[Utility.Random(names.Length)];
			Title = titles[Utility.Random(titles.Length)];

            Body = 0x190;
            Hue = 1108; // Dark grey

            SpeechHue = 1154;

            // Outfit - cruel, practical, and intimidating
            AddItem(new StuddedChest() { Hue = 1107 });
            AddItem(new StuddedLegs() { Hue = 1107 });
            AddItem(new StuddedGloves() { Hue = 1107 });
            AddItem(new HalfApron() { Hue = 1102 });
            AddItem(new OrcHelm()); // Barbaric-looking
            AddItem(new Boots() { Hue = 1102 });

            // Weapon - Scepter for "authority" + brutality
            AddItem(new Scepter());

            // Stats
            SetStr(550, 600);
            SetDex(120, 150);
            SetInt(80, 100);

            SetHits(200, 250);
            SetDamage(22, 30);

            SetSkill(SkillName.MagicResist, 75.0, 90.0);
            SetSkill(SkillName.Tactics, 90.0, 110.0);
            SetSkill(SkillName.Macing, 100.0, 120.0);

            Fame = 14000;
            Karma = -14000;

            VirtualArmor = 60;

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

                if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 10))
                {
                    int phrase = Utility.Random(6);

                    switch (phrase)
                    {
                        case 0: this.Say("Work or perish! The mines demand flesh."); break;
                        case 1: this.Say("Another strong back for the pits..."); break;
                        case 2: this.Say("Even your bones will serve Minax."); break;
                        case 3: this.Say("*snaps whip across the stones*"); break;
                        case 4: this.Say("Your struggle amuses me."); break;
                        case 5: this.Say("The weak find purpose in servitude."); break;
                    }

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }
            }
        }

        public override int Damage(int amount, Mobile from)
        {
            if (from != null && from.Map == this.Map && from.InRange(this, 8) && Utility.RandomBool())
            {
                int phrase = Utility.Random(4);

                switch (phrase)
                {
                    case 0: this.Say("Your defiance is pointless."); break;
                    case 1: this.Say("I will break you like the others."); break;
                    case 2: this.Say("*laughs coldly*"); break;
                    case 3: this.Say("Pain is but the path to obedience."); break;
                }

                m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
            }

            return base.Damage(amount, from);
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (Utility.RandomDouble() < 0.25)
            {
                this.Say("On your knees, worm!");
            }
        }

        public override void OnGotMeleeAttack(Mobile attacker)
        {
            base.OnGotMeleeAttack(attacker);

            if (Utility.RandomDouble() < 0.2)
            {
                this.Say("Resistance is temporary. Chains are forever.");
            }
        }

        public override void OnDeath(Container c)
        {
            this.Say("You may have slain me... but the mines will claim you yet...");
            base.OnDeath(c);
        }

        public override void GenerateLoot()
        {
            PackGold(300, 500);
            PackItem(new IronOre(Utility.RandomMinMax(10, 20)));

            if (Utility.RandomDouble() < 0.2)
                PackItem(new Whip()); // symbolic drop—Overseer’s whip

            if (Utility.RandomDouble() < 0.1)
                PackItem(new RelicFragment()); // Rare loot
        }

        public MineOverseer(Serial serial) : base(serial) { }

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
