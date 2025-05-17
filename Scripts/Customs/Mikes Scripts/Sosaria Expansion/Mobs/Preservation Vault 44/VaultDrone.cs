using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("a drone's wreckage")]
    public class VaultDrone : BaseCreature
    {
        private bool m_FieldActive;

        [Constructable]
        public VaultDrone()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Vault Drone";
            Body = 0x2F5;
            Hue = 2966; // Unique tech-magic hue

            SetStr(900, 1000);
            SetDex(80, 100);
            SetInt(120, 140);

            SetHits(600, 700);

            SetDamage(18, 26);

            SetResistance(ResistanceType.Physical, 65, 75);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 20, 30);
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.MagicResist, 95.0, 105.0);
            SetSkill(SkillName.Tactics, 95.0, 105.0);
            SetSkill(SkillName.Wrestling, 95.0, 105.0);

            Fame = 20000;
            Karma = -20000;
            VirtualArmor = 70;

            PackItem(new PowerCrystal());
            PackItem(new ArcaneGem());

            if (Utility.RandomBool())
                PackItem(new ClockworkAssembly());

            m_FieldActive = CanUseField;
        }

        public VaultDrone(Serial serial)
            : base(serial)
        {
        }

        public override bool AutoDispel => true;
        public override bool IsScaryToPets => true;
        public override bool BardImmune => !Core.AOS;
        public override Poison PoisonImmune => Poison.Lethal;

        public bool FieldActive => m_FieldActive;

        public bool CanUseField => Hits >= HitsMax * 9 / 10;

        public override void AlterMeleeDamageFrom(Mobile from, ref int damage)
        {
            if (m_FieldActive)
                damage = 0; // Immune to melee when shield is up
        }

        public override void AlterSpellDamageFrom(Mobile from, ref int damage)
        {
            if (!m_FieldActive)
                damage = 0; // Immune to spells when shield is down
        }

        public override void OnDamagedBySpell(Mobile from)
        {
            if (from != null && from.Alive && 0.4 > Utility.RandomDouble())
                SendEBolt(from);

            if (!m_FieldActive)
            {
                FixedParticles(0, 10, 0, 0x2522, EffectLayer.Waist);
            }
            else if (m_FieldActive && !CanUseField)
            {
                m_FieldActive = false;
                FixedParticles(0x3735, 1, 30, 0x251F, EffectLayer.Waist);
            }
        }

        public override void OnGotMeleeAttack(Mobile attacker)
        {
            base.OnGotMeleeAttack(attacker);

            if (m_FieldActive)
            {
                FixedParticles(0x376A, 20, 10, 0x2530, EffectLayer.Waist);
                PlaySound(0x2F4);
                attacker.SendAsciiMessage("The drone's shielding deflects your strike!");
            }

            if (attacker != null && attacker.Alive && attacker.Weapon is BaseRanged && 0.4 > Utility.RandomDouble())
                SendEBolt(attacker);
        }

        public override void OnThink()
        {
            base.OnThink();

            if (!m_FieldActive && !IsHurt())
                m_FieldActive = true;
        }

        public override bool Move(Direction d)
        {
            bool move = base.Move(d);

            if (move && m_FieldActive && Combatant != null)
                FixedParticles(0, 10, 0, 0x2530, EffectLayer.Waist);

            return move;
        }

        public void SendEBolt(Mobile to)
        {
            MovingParticles(to, 0x379F, 7, 0, false, true, 0xBE3, 0xFCB, 0x211);
            to.PlaySound(0x229);
            DoHarmful(to);
            AOS.Damage(to, this, 50, 0, 0, 0, 0, 100);
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
            AddLoot(LootPack.Rich);

            if (Utility.RandomDouble() < 0.001) // 1 in 1000 rare relic
                PackItem(new ExodusVanguardPlate());
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            // 30% chance to drop quest item
            if (Utility.RandomDouble() < 0.3)
                c.DropItem(new ImperiumPowerCore());

            // Occasional rare tech item
            if (Utility.RandomDouble() < 0.2)
                c.DropItem(new MechanicalComponent());
        }

        public override int GetIdleSound() => 0x218;
        public override int GetAngerSound() => 0x26C;
        public override int GetDeathSound() => 0x211;
        public override int GetAttackSound() => 0x232;
        public override int GetHurtSound() => 0x140;

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
            m_FieldActive = CanUseField;
        }
    }
}
