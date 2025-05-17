using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("a corroded red solen corpse")]
    public class CorrodedRedSolen : BaseCreature
    {
        private DateTime m_NextCorrosiveSpray;
        private DateTime m_NextAcidBurst;
        private DateTime m_NextAuraTick;

        private const int UniqueHue = 2050; // A sickly, corroded green

        [Constructable]
        public CorrodedRedSolen()
            : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.3, 0.5)
        {
            Name = "a corroded red solen";
            Body = 782;
            BaseSoundID = 959;
            Hue = UniqueHue;

            // Stats
            SetStr(300, 350);
            SetDex(120, 150);
            SetInt(80, 100);

            SetHits(500, 600);
            SetStam(200, 250);
            SetMana(50, 75);

            SetDamage(10, 20);

            // Damage types
            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Poison, 50);

            // Resistances
            SetResistance(ResistanceType.Physical, 30, 45);
            SetResistance(ResistanceType.Fire,     25, 35);
            SetResistance(ResistanceType.Cold,     20, 30);
            SetResistance(ResistanceType.Poison,   50, 60);
            SetResistance(ResistanceType.Energy,   20, 30);

            // Skills
            SetSkill(SkillName.MagicResist, 80.0);
            SetSkill(SkillName.Tactics,     90.0);
            SetSkill(SkillName.Wrestling,   90.0);

            Fame = 8000;
            Karma = -8000;

            VirtualArmor = 50;
            ControlSlots = 3;

            // Initialize cooldowns
            m_NextCorrosiveSpray = DateTime.UtcNow + TimeSpan.FromSeconds(10);
            m_NextAcidBurst       = DateTime.UtcNow + TimeSpan.FromSeconds(5);
            m_NextAuraTick        = DateTime.UtcNow + TimeSpan.FromSeconds(1);

            // Basic loot
            PackItem(new GreaterPoisonPotion());
            PackItem(new ZoogiFungus(Utility.RandomMinMax(5, 10)));
        }

        public override void OnThink()
        {
            base.OnThink();

            // 1. Corrosive Aura (pulse every second)
            if (DateTime.UtcNow >= m_NextAuraTick)
            {
                m_NextAuraTick = DateTime.UtcNow + TimeSpan.FromSeconds(1);

                if (Map == null) return;
				IPooledEnumerable eable = Map.GetMobilesInRange(Location, 2);
				try
				{
					foreach (Mobile m in eable)
					{
						if (m != this && m.Alive && CanBeHarmful(m, false) && m is Mobile auraTarget)
						{
							auraTarget.SendMessage("The air crackles with corrosive venom!");
							AOS.Damage(auraTarget, this, Utility.RandomMinMax(5, 10), 0, 0, 0, 0, 100);
						}
					}
				}
				finally
				{
					eable.Free(); // This is critical to release pooled resources
				}

            }

            // 2. Corrosive Spray (ranged acid cone)
            if (Combatant is Mobile sprayTarget && Alive && DateTime.UtcNow >= m_NextCorrosiveSpray && InRange(sprayTarget.Location, 8))
            {
                BeginCorrosiveSpray(sprayTarget);
                m_NextCorrosiveSpray = DateTime.UtcNow + TimeSpan.FromSeconds(20);
            }
        }

        public override void OnGotMeleeAttack(Mobile attacker)
        {
            if (attacker is Mobile target && DateTime.UtcNow >= m_NextAcidBurst)
            {
                AcidBurst(target);
                m_NextAcidBurst = DateTime.UtcNow + TimeSpan.FromSeconds(10);
            }
            base.OnGotMeleeAttack(attacker);
        }

        public override void OnDamagedBySpell(Mobile attacker)
        {
            if (attacker is Mobile target && DateTime.UtcNow >= m_NextAcidBurst)
            {
                AcidBurst(target);
                m_NextAcidBurst = DateTime.UtcNow + TimeSpan.FromSeconds(10);
            }
            base.OnDamagedBySpell(attacker);
        }

        // --- Corrosive Spray: cone of acid that deals heavy damage + armor corrosion debuff ---
        public void BeginCorrosiveSpray(Mobile target)
        {
            PlaySound(0x118);
            MovingEffect(target, 0x36D4, 1, 0, false, false, 0x3F, 0);
            Timer.DelayCall(TimeSpan.FromSeconds(GetDistanceToSqrt(target) / 5.0), () => EndCorrosiveSpray(target));
        }

        private void EndCorrosiveSpray(Mobile target)
        {
            if (target == null || target.Deleted || !target.Alive) return;
            if (!CanBeHarmful(target, false)) return;

            target.SendMessage("A torrent of corrosive acid batters you, eating into your defenses!");
            AOS.Damage(target, this, Utility.RandomMinMax(80, 100), 0, 0, 0, 0, 100);
            target.FixedParticles(0x36BD, 1, 20, 9502, EffectLayer.Waist);
            target.PlaySound(0x4F);

            // Temporary armor reduction (physical resist debuff)
            var debuff = new ResistanceMod(ResistanceType.Physical, -15);
            target.AddResistanceMod(debuff);
            Timer.DelayCall(TimeSpan.FromSeconds(15), () => { target.RemoveResistanceMod(debuff); });
        }

        // --- Acid Burst: splash immediate acid around a single target ---
        public void AcidBurst(Mobile target)
        {
            PlaySound(0x64F);
            target.FixedEffect(0x372A, 10, 30, 0x12E, 0);
            if (Utility.RandomDouble() < 0.3)
                target.ApplyPoison(this, Poison.Lethal);
            AOS.Damage(target, this, Utility.RandomMinMax(20, 40), 0, 0, 0, 0, 100);
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            // On death, spawn toxic gas tiles in a small radius
            if (Map == null) return;

            for (int i = 0; i < 6; i++)
            {
                int x = X + Utility.RandomMinMax(-3, 3);
                int y = Y + Utility.RandomMinMax(-3, 3);
                int z = Z;

                if (!Map.CanFit(x, y, z, 16, false, false))
                    z = Map.GetAverageZ(x, y);

                var gas = new ToxicGasTile();
                gas.Hue = UniqueHue;
                gas.MoveToWorld(new Point3D(x, y, z), Map);
            }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(4, 8));

            // 3% chance to drop a rare Corroded Core
            if (Utility.RandomDouble() < 0.03)
                PackItem(new TreadOfTheThornMarch());  // Assume CorrodedCore is defined elsewhere
        }

        public CorrodedRedSolen(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Re-init timers on load
            m_NextCorrosiveSpray = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextAcidBurst       = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 8));
            m_NextAuraTick        = DateTime.UtcNow + TimeSpan.FromSeconds(1);
        }
    }
}
