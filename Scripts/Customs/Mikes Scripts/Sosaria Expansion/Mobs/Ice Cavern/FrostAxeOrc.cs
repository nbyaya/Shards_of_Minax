using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a frost‐axe orc corpse")]
    public class FrostAxeOrc : BaseCreature
    {
        private DateTime m_NextFreezeStrike;
        private DateTime m_NextIceHowl;
        private DateTime m_NextShatterArmor;
        private DateTime m_NextSummonMinions;

        [Constructable]
        public FrostAxeOrc()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Frost‐axe Orc";
            Body = 7;
            BaseSoundID = 0x45A;
            Hue = 1150; // Icy blue unique hue


            SetStr(350, 420);
            SetDex(150, 180);
            SetInt(80, 110);

            SetHits(600, 750);
            SetStam(300, 400);
            SetMana(100, 150);

            SetDamage(15, 27);
            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Cold, 50);

            SetResistance(ResistanceType.Physical, 40, 55);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 25, 35);

            SetSkill(SkillName.MagicResist, 80.1, 100.0);
            SetSkill(SkillName.Tactics, 100.1, 120.0);
            SetSkill(SkillName.Wrestling, 100.1, 120.0);
            SetSkill(SkillName.Anatomy, 60.0, 90.0);

            Fame = 16000;
            Karma = -16000;

            VirtualArmor = 60;

            PackItem(new IceAxe()); // Custom item
            AddLoot(LootPack.Rich);
        }

        public FrostAxeOrc(Serial serial) : base(serial) { }

        public override bool CanRummageCorpses => true;
        public override int Meat => 1;
        public override OppositionGroup OppositionGroup => OppositionGroup.SavagesAndOrcs;

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null)
                return;

            if (!Alive)
                return;

            if (Combatant is Mobile target)
            {
                if (DateTime.UtcNow >= m_NextFreezeStrike)
                {
                    FreezeStrike(target);
                    m_NextFreezeStrike = DateTime.UtcNow + TimeSpan.FromSeconds(20);
                }

                if (DateTime.UtcNow >= m_NextIceHowl)
                {
                    IceHowl();
                    m_NextIceHowl = DateTime.UtcNow + TimeSpan.FromSeconds(30);
                }

                if (DateTime.UtcNow >= m_NextShatterArmor)
                {
                    TryShatterArmor(target);
                    m_NextShatterArmor = DateTime.UtcNow + TimeSpan.FromSeconds(45);
                }

                if (DateTime.UtcNow >= m_NextSummonMinions)
                {
                    SummonIceOrcMinions();
                    m_NextSummonMinions = DateTime.UtcNow + TimeSpan.FromSeconds(60);
                }
            }
        }

        private void FreezeStrike(Mobile target)
        {
            if (target.Frozen || !target.Alive)
                return;

            PublicOverheadMessage(MessageType.Emote, 0x480, false, "*smashes the ground with a frost-charged roar*");
            target.Freeze(TimeSpan.FromSeconds(3));
            target.SendMessage("You feel your limbs stiffen from the icy blow!");
            AOS.Damage(target, this, Utility.RandomMinMax(20, 30), 0, 100, 0, 0, 0);
            target.PlaySound(0x10B);
            Effects.SendLocationEffect(target.Location, target.Map, 0x374A, 30, 10);
        }

        private void IceHowl()
        {
            PublicOverheadMessage(MessageType.Emote, 0x480, false, "*howls, summoning icy winds from the mountain*");
            PlaySound(0x64C);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && m is Mobile mob && mob.Player)
                {
                    mob.SendMessage("A chilling howl pierces through your soul!");
                    AOS.Damage(mob, this, Utility.RandomMinMax(10, 20), 0, 100, 0, 0, 0);
                    mob.Stam -= Utility.RandomMinMax(10, 20);
                }
            }
        }

        private void TryShatterArmor(Mobile target)
        {
            if (target.FindItemOnLayer(Layer.OuterTorso) is Item armor && armor is BaseArmor ba)
            {
                PublicOverheadMessage(MessageType.Emote, 0x480, false, "*brings down his axe with shattering force!*");
                AOS.Damage(target, this, Utility.RandomMinMax(15, 25), 100, 0, 0, 0, 0);
                ba.HitPoints = Math.Max(0, ba.HitPoints - 20);

                if (ba.HitPoints == 0)
                {
                    armor.Delete();
                    target.SendMessage("Your armor shatters into pieces!");
                }
            }
        }

        private void SummonIceOrcMinions()
        {
            PublicOverheadMessage(MessageType.Emote, 0x480, false, "*raises his axe, calling frostkin from the snow*");
            for (int i = 0; i < 2; i++)
            {
                IceOrcMinion minion = new IceOrcMinion();
                minion.Team = this.Team;
                minion.FightMode = FightMode.Closest;
                minion.MoveToWorld(this.Location, this.Map);
                minion.Combatant = this.Combatant;
            }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average, 2);
            AddLoot(LootPack.Gems);
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

    public class IceOrcMinion : BaseCreature
    {
        [Constructable]
        public IceOrcMinion()
            : base(AIType.AI_Melee, FightMode.Closest, 8, 1, 0.2, 0.4)
        {
            Name = "an ice minion";
            Body = 153;
            BaseSoundID = 268;
            Hue = 0x480;

            SetStr(80, 100);
            SetDex(80, 100);
            SetInt(10);

            SetHits(120);
            SetDamage(5, 10);

            SetDamageType(ResistanceType.Cold, 100);

            SetResistance(ResistanceType.Cold, 80);
            SetResistance(ResistanceType.Physical, 20);

            VirtualArmor = 15;

            Fame = 0;
            Karma = -100;
        }

        public IceOrcMinion(Serial serial) : base(serial) { }

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

    public class IceAxe : ExecutionersAxe
    {
        [Constructable]
        public IceAxe()
        {
            Name = "a frost-etched axe";
            Hue = 1150;
            Attributes.WeaponSpeed = 25;
            Attributes.WeaponDamage = 35;
            LootType = LootType.Blessed;
        }

        public IceAxe(Serial serial) : base(serial) { }

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
