using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a clockwork engineer")]
    public class ClockworkEngineer : BaseCreature
    {
        private TimeSpan m_SpawnDelay = TimeSpan.FromSeconds(20.0); // time between clockwork soldier spawns
        public DateTime m_NextSpawnTime;

        [Constructable]
        public ClockworkEngineer() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();
            Body = 0x190;
            Name = NameList.RandomName("male");
            Title = " the Clockwork Engineer";
			Team = 2;

            Item robe = new Robe();
            robe.Hue = 1150; // dark grey
            AddItem(robe);

            Item boots = new Boots();
            AddItem(boots);

            Item goggles = new Item(0x1F06);
            goggles.Hue = 2406; // red
            goggles.Layer = Layer.Helm;
            goggles.Movable = false;
            AddItem(goggles);

            SetStr(500, 700);
            SetDex(90, 120);
            SetInt(250, 300);

            SetHits(400, 600);

            SetDamage(5, 15);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 20, 30);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 20, 30);

            SetSkill(SkillName.Anatomy, 60.0, 80.0);
            SetSkill(SkillName.EvalInt, 80.0, 100.0);
            SetSkill(SkillName.Magery, 85.0, 95.0);
            SetSkill(SkillName.MagicResist, 85.0, 100.0);
            SetSkill(SkillName.Tactics, 60.0, 80.0);
            SetSkill(SkillName.Wrestling, 70.0, 90.0);

            Fame = 4500;
            Karma = -4500;

            VirtualArmor = 45;

            m_NextSpawnTime = DateTime.Now + m_SpawnDelay;
        }

        public override void OnThink()
        {
            if (DateTime.Now >= m_NextSpawnTime)
            {
                Mobile combatant = this.Combatant as Mobile;

                if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 8))
                {
                    this.Say("Constructing a clockwork soldier!");

                    ClockworkSoldier soldier = new ClockworkSoldier();
                    soldier.MoveToWorld(this.Location, this.Map);
                    soldier.Combatant = combatant;

                    m_NextSpawnTime = DateTime.Now + m_SpawnDelay;
                }

                base.OnThink();
            }
        }

        public override void GenerateLoot()
        {
            PackGem();
            PackGold(150, 200);
            AddLoot(LootPack.Rich);

            int phrase = Utility.Random(2);

            switch (phrase)
            {
                case 0: this.Say(true, "My machines... have failed me..."); break;
                case 1: this.Say(true, "You may have won this time..."); break;
            }

            PackItem(new Gears(Utility.RandomMinMax(10, 20)));
        }

        public ClockworkEngineer(Serial serial) : base(serial)
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

    public class ClockworkSoldier : BaseCreature
    {
        [Constructable]
        public ClockworkSoldier() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Body = 0x33; // metallic body
            Name = "clockwork soldier";

            SetStr(200, 300);
            SetDex(50, 70);
            SetInt(30, 50);

            SetHits(150, 200);

            SetDamage(10, 20);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 20, 30);
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 20, 30);

            SetSkill(SkillName.Anatomy, 50.0, 70.0);
            SetSkill(SkillName.MagicResist, 60.0, 80.0);
            SetSkill(SkillName.Tactics, 70.0, 90.0);
            SetSkill(SkillName.Wrestling, 80.0, 100.0);

            Fame = 2500;
            Karma = -2500;

            VirtualArmor = 35;
        }

        public ClockworkSoldier(Serial serial) : base(serial)
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
