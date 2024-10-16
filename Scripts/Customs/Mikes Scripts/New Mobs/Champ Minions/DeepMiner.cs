using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a deep miner")]
    public class DeepMiner : BaseCreature
    {
        private TimeSpan m_TrapDelay = TimeSpan.FromSeconds(20.0); // time between setting traps
        public DateTime m_NextTrapTime;

        [Constructable]
        public DeepMiner() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Deep Miner";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Deep Miner";
            }

            Item shirt = new Shirt();
            AddItem(shirt);

            Item pants = new LongPants(Utility.RandomNeutralHue());
            AddItem(pants);

            Item boots = new Boots(Utility.RandomNeutralHue());
            AddItem(boots);

            Item pickaxe = new Pickaxe();
            AddItem(pickaxe);
            pickaxe.Movable = false;

            if (!this.Female)
            {
                Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
                hair.Hue = Utility.RandomHairHue();
                hair.Layer = Layer.Hair;
                hair.Movable = false;
                AddItem(hair);
            }

            SetStr(700, 900);
            SetDex(150, 200);
            SetInt(100, 150);

            SetHits(500, 700);

            SetDamage(15, 25);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 50, 70);
            SetResistance(ResistanceType.Fire, 30, 50);
            SetResistance(ResistanceType.Cold, 30, 50);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 20, 40);

            SetSkill(SkillName.Anatomy, 50.0, 70.0);
            SetSkill(SkillName.Tactics, 80.1, 100.0);
            SetSkill(SkillName.Wrestling, 80.1, 100.0);
            SetSkill(SkillName.Mining, 90.1, 100.0);

            Fame = 5000;
            Karma = -5000;

            VirtualArmor = 40;

            m_NextTrapTime = DateTime.Now + m_TrapDelay;
        }

        public override bool AlwaysMurderer { get { return true; } }
        public override bool CanRummageCorpses { get { return true; } }
        public override bool ShowFameTitle { get { return false; } }
        public override bool ClickTitle { get { return true; } }

        public override void OnThink()
        {
            if (DateTime.Now >= m_NextTrapTime)
            {
                // Code to set traps or surprise attacks
                Mobile combatant = this.Combatant as Mobile;

                if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 8))
                {
                    this.Say(true, "Feel the ground tremble beneath you!");
                    // Custom trap logic goes here, e.g., creating a trap item or effect
                    // Example: create an explosion at the combatant's location
                    Effects.SendLocationEffect(combatant.Location, combatant.Map, 0x36BD, 20, 10, 0, 0);
                    combatant.Damage(Utility.RandomMinMax(10, 20), this);

                    m_NextTrapTime = DateTime.Now + m_TrapDelay;
                }

                base.OnThink();
            }
        }

        public override void GenerateLoot()
        {
            PackGem();
            PackGold(150, 200);
            AddLoot(LootPack.Average);

            int phrase = Utility.Random(2);

            switch (phrase)
            {
                case 0: this.Say(true, "You haven't seen the last of the miners!"); break;
                case 1: this.Say(true, "We'll be back, stronger than ever!"); break;
            }

            PackItem(new IronIngot(Utility.RandomMinMax(5, 15)));
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
                        case 0: this.Say(true, "You think a miner is so easily defeated?!"); break;
                        case 1: this.Say(true, "The depths protect me!"); break;
                        case 2: this.Say(true, "You'll regret that!"); break;
                        case 3: this.Say(true, "I've dug deeper than you can imagine!"); break;
                    }
                }
            }

            return base.Damage(amount, from);
        }

        public DeepMiner(Serial serial) : base(serial)
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
