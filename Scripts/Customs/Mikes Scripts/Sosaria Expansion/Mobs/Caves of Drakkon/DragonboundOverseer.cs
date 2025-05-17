using System;
using Server.Items;

namespace Server.Mobiles
{
    public class DragonboundOverseer : BaseCreature
    {
        [Constructable]
        public DragonboundOverseer()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Dragonbound Overseer";
            Title = "Enforcer of Drakkon's Will";
            Hue = 2075; // Unique scaled hue

            Body = 0x190;
            SpeechHue = 34;

            SetStr(420, 450);
            SetDex(160, 180);
            SetInt(180, 200);

            SetDamage(12, 18);
            SetDamageType(ResistanceType.Fire, 40);
            SetDamageType(ResistanceType.Physical, 60);

            SetResistance(ResistanceType.Physical, 45, 55);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 25, 35);
            SetResistance(ResistanceType.Poison, 20, 30);
            SetResistance(ResistanceType.Energy, 25, 35);

            SetSkill(SkillName.MagicResist, 100.0);
            SetSkill(SkillName.Swords, 120.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Anatomy, 110.0);

            Fame = 7000;
            Karma = -7000;

            VirtualArmor = 50;

            AddItem(new PlateChest() { Hue = 2075 });
            AddItem(new PlateLegs() { Hue = 2075 });
            AddItem(new PlateArms() { Hue = 2075 });
            AddItem(new PlateGloves() { Hue = 2075 });
            AddItem(new VikingSword());
            AddItem(new MetalShield() { Hue = 1358 });

            Utility.AssignRandomHair(this);
        }

        public override bool AlwaysMurderer => true;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
            PackItem(new DragonchainShard()); // Unique item drop
        }

        public override void OnGotMeleeAttack(Mobile attacker)
        {
            base.OnGotMeleeAttack(attacker);

            if (Utility.RandomDouble() < 0.1)
            {
                attacker.SendMessage(0x22, "The Overseer's blade burns with Drakkon's fury!");
                AOS.Damage(attacker, this, Utility.RandomMinMax(10, 20), 0, 100, 0, 0, 0);
            }
        }

        public DragonboundOverseer(Serial serial) : base(serial) { }

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
