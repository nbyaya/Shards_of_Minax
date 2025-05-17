using System;
using Server;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using System.Collections.Generic;

namespace Server.Mobiles
{
    [CorpseName("the corpse of the Frozen Fenrir")]
    public class FrozenFenrir : BaseCreature
    {
        private DateTime m_NextCryoHowl;
        private DateTime m_NextFrostSpike;
        private bool m_AuraActive;

        [Constructable]
        public FrozenFenrir()
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "Frozen Fenrir";
            Body = 98;
            Hue = 1150; // Unique frosty blue
            BaseSoundID = 229;

            SetStr(600, 800);
            SetDex(120, 180);
            SetInt(250, 350);

            SetHits(1200, 1600);

            SetDamage(20, 28);
            SetDamageType(ResistanceType.Physical, 30);
            SetDamageType(ResistanceType.Cold, 70);

            SetResistance(ResistanceType.Physical, 55, 65);
            SetResistance(ResistanceType.Cold, 80, 90);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Energy, 35, 45);
            SetResistance(ResistanceType.Poison, 30, 40);

            SetSkill(SkillName.Magery, 90.1, 100.0);
            SetSkill(SkillName.MagicResist, 100.0, 130.0);
            SetSkill(SkillName.Tactics, 90.0, 100.0);
            SetSkill(SkillName.Wrestling, 95.0, 110.0);
            SetSkill(SkillName.EvalInt, 90.0, 100.0);

            Fame = 26000;
            Karma = -26000;
            VirtualArmor = 60;

            Tamable = false;
            ControlSlots = 0;

            m_NextCryoHowl = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 20));
            m_NextFrostSpike = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 30));
        }

        public FrozenFenrir(Serial serial) : base(serial) { }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (DateTime.UtcNow >= m_NextCryoHowl)
                    CryoHowl();

                if (DateTime.UtcNow >= m_NextFrostSpike)
                    LaunchFrostSpike();

                ApplyFreezingAura();
            }
        }

        private void CryoHowl()
        {
            PublicOverheadMessage(MessageType.Regular, 0x480, true, "* The Frozen Fenrir unleashes a soul-chilling howl! *");
            PlaySound(0x554); // Dread howl sound

            for (int i = 0; i < 2; i++)
            {
                BaseCreature mirrorWolf = new IceHound
                {
                    Name = "Fenrir Echo",
                    Hue = 1152,
                    ControlSlots = 0,
                    Team = this.Team
                };

                mirrorWolf.MoveToWorld(this.Location, this.Map);
                mirrorWolf.Combatant = this.Combatant;
            }

            m_NextCryoHowl = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 45));
        }

        private void LaunchFrostSpike()
        {
            if (Combatant != null)
            {
                PublicOverheadMessage(MessageType.Regular, 0x480, true, "* The ground cracks beneath you as an icy spike erupts! *");
                PlaySound(0x10B); // Impact sound

                if (Combatant is Mobile target && target.Alive)
                {
                    Effects.SendLocationEffect(target.Location, target.Map, 0x3709, 20, 10);
                    AOS.Damage(target, this, Utility.RandomMinMax(30, 45), 0, 100, 0, 0, 0);

                    target.SendMessage("A jagged spike of ice tears through you!");
                    target.Freeze(TimeSpan.FromSeconds(3));
                }
            }

            m_NextFrostSpike = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 35));
        }

        private void ApplyFreezingAura()
        {
            if (Combatant is Mobile target && target.Alive && Utility.RandomDouble() < 0.05)
            {
                target.SendMessage("Your limbs stiffen from the unnatural cold!");
                target.Stam -= Utility.RandomMinMax(5, 15);
                target.Mana -= Utility.RandomMinMax(5, 10);
                target.ApplyPoison(this, Poison.Regular);
                Effects.SendTargetEffect(target, 0x373A, 10, 1);
            }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.FilthyRich, 1);
            AddLoot(LootPack.Gems, 8);

            if (Utility.RandomDouble() < 0.02) // 2% rare drop
            {
                PackItem(new FrozenHeartCrystal());
            }
        }

        public override bool AutoDispel => true;
        public override int TreasureMapLevel => 4;

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

    public class FrozenHeartCrystal : Item
    {
        [Constructable]
        public FrozenHeartCrystal() : base(0x1F19)
        {
            Name = "Frozen Heart Crystal";
            Hue = 1152;
            Weight = 1.0;
            LootType = LootType.Regular;
        }

        public FrozenHeartCrystal(Serial serial) : base(serial) { }

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
