using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a net caster")]
    public class NetCaster : BaseCreature
    {
        private TimeSpan m_NetDelay = TimeSpan.FromSeconds(15.0); // time between net casting
        public DateTime m_NextNetTime;

        [Constructable]
        public NetCaster() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();
			Team = 2;

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Net Caster";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Net Caster";
            }

            Item robe = new Robe();
            robe.Hue = 0x497;
            AddItem(robe);

            Item sandals = new Sandals();
            sandals.Hue = 0x1BB;
            AddItem(sandals);

            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;
            AddItem(hair);

            if (!this.Female)
            {
                Item beard = new Item(Utility.RandomList(0x203E, 0x2041));
                beard.Hue = hair.Hue;
                beard.Layer = Layer.FacialHair;
                beard.Movable = false;
                AddItem(beard);
            }

            SetStr(600, 800);
            SetDex(100, 150);
            SetInt(200, 250);

            SetHits(500, 700);

            SetDamage(8, 14);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 40, 60);
            SetResistance(ResistanceType.Fire, 30, 50);
            SetResistance(ResistanceType.Cold, 30, 50);
            SetResistance(ResistanceType.Poison, 50, 70);
            SetResistance(ResistanceType.Energy, 40, 60);

            SetSkill(SkillName.Anatomy, 50.0, 75.0);
            SetSkill(SkillName.MagicResist, 70.0, 90.0);
            SetSkill(SkillName.Tactics, 60.0, 80.0);
            SetSkill(SkillName.Wrestling, 70.0, 90.0);

            Fame = 4500;
            Karma = -4500;

            VirtualArmor = 50;

            m_NextNetTime = DateTime.Now + m_NetDelay;
        }

        public override bool AlwaysMurderer { get { return true; } }
        public override bool CanRummageCorpses { get { return true; } }
        public override bool ShowFameTitle { get { return false; } }
        public override bool ClickTitle { get { return true; } }

        public override void OnThink()
        {
            if (DateTime.Now >= m_NextNetTime)
            {
                Mobile combatant = this.Combatant as Mobile;

                if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 8))
                {
                    this.Say(true, "You won't escape my nets!");

                    // Apply net effect
                    combatant.SendMessage("You have been caught in a net!");
                    combatant.Freeze(TimeSpan.FromSeconds(5.0)); // Freeze for 5 seconds
                    combatant.AddStatMod(new StatMod(StatType.Dex, "NetCasterDexMod", -10, TimeSpan.FromSeconds(10.0))); // Reduce dexterity for 10 seconds

                    m_NextNetTime = DateTime.Now + m_NetDelay;
                }
            }

            base.OnThink();
        }

        public override void GenerateLoot()
        {
            PackGem();
            PackGold(150, 200);
            AddLoot(LootPack.Rich);

            PackItem(new Bandage(Utility.RandomMinMax(10, 20)));
        }

        public NetCaster(Serial serial) : base(serial)
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
