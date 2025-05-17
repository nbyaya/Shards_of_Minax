using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a mountain minotaur corpse")]
    public class MountainMinotaur : BaseCreature
    {
        private DateTime m_NextStompTime;
        private DateTime m_NextChargeTime;
        private DateTime m_NextRoarTime;

        private const int UniqueHue = 1175; // Steel‑gray hue

        [Constructable]
        public MountainMinotaur() 
            : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "a mountain minotaur";
            Body = 0x118;
            BaseSoundID = 0x61A; // Minotaur roar
            Hue = UniqueHue;

            // --- Enhanced Stats ---
            SetStr(800, 900);
            SetDex(200, 250);
            SetInt(120, 150);

            SetHits(2000, 2400);
            SetStam(300, 350);
            SetMana(100, 150);

            SetDamage(25, 35);
            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 85, 90);
            SetResistance(ResistanceType.Fire, 70, 75);
            SetResistance(ResistanceType.Cold, 70, 75);
            SetResistance(ResistanceType.Poison, 70, 75);
            SetResistance(ResistanceType.Energy, 80, 85);

            SetSkill(SkillName.Wrestling, 120.0, 140.0);
            SetSkill(SkillName.Tactics, 110.0, 120.0);
            SetSkill(SkillName.MagicResist, 100.0, 110.0);

            Fame = 22000;
            Karma = -22000;
            VirtualArmor = 90;
            ControlSlots = 5;

            // Initialize cooldowns
            m_NextStompTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextChargeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_NextRoarTime   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));

            // Basic loot
            PackItem(new IronIngot(Utility.RandomMinMax(5, 10)));
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || Map == null || !Alive)
                return;

            // Earthen Stomp: AoE around itself
            if (DateTime.UtcNow >= m_NextStompTime && InRange(Combatant.Location, 5))
            {
                EarthenStomp();
                m_NextStompTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
            }
            // Bull Charge: leap attack
            else if (DateTime.UtcNow >= m_NextChargeTime 
                     && GetDistanceToSqrt(Combatant) >= 2 && GetDistanceToSqrt(Combatant) <= 12
                     && Combatant is Mobile)
            {
                BullCharge();
                m_NextChargeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }
            // Savage Roar: stamina drain in radius
            else if (DateTime.UtcNow >= m_NextRoarTime)
            {
                SavageRoar();
                m_NextRoarTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
        }

        // --- Ability #1: Earthen Stomp ---
        private void EarthenStomp()
        {
            PlaySound(0x214); // Earthquake sound
            FixedParticles(0x376A, 10, 30, 5052, UniqueHue, 0, EffectLayer.CenterFeet);

            IPooledEnumerable eable = Map.GetMobilesInRange(Location, 5);
            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                {
                    DoHarmful(m);
                    AOS.Damage(m, this, Utility.RandomMinMax(40, 60), 100, 0, 0, 0, 0);

                    if (m is Mobile targetMobile)
                    {
                        targetMobile.Freeze(TimeSpan.FromSeconds(2));
                        targetMobile.SendMessage("You are stunned by the earth-shattering stomp!");
                    }

                    // Spawn earth fissure hazard
                    EarthquakeTile tile = new EarthquakeTile();
                    tile.MoveToWorld(m.Location, Map);
                }
            }
            eable.Free();
        }

        // --- Ability #2: Bull Charge ---
        private void BullCharge()
        {
            if (Combatant is Mobile target)
            {
                Say("*The ground trembles as it charges!*");
                PlaySound(0x220);

                // Teleport into position
                Point3D loc = target.Location;
                if (Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    MoveToWorld(loc, Map);

                FixedParticles(0x3728, 1, 15, 5032, UniqueHue, 0, EffectLayer.Waist);

                DoHarmful(target);
                AOS.Damage(target, this, Utility.RandomMinMax(80, 120), 100, 0, 0, 0, 0);

                // Simple knock‑back
				Direction knockbackDir = GetDirectionTo(target);
				Point3D knockbackLoc = target.Location;

				int dx = knockbackLoc.X - X;
				int dy = knockbackLoc.Y - Y;

				int pushX = dx != 0 ? dx / Math.Abs(dx) : 0;
				int pushY = dy != 0 ? dy / Math.Abs(dy) : 0;

				Point3D newLoc = new Point3D(target.X + pushX, target.Y + pushY, target.Z);

				if (Map.CanFit(newLoc, 16, false, false))
					target.MoveToWorld(newLoc, Map);

            }
        }

        // --- Ability #3: Savage Roar ---
        private void SavageRoar()
        {
            Say("*ROOOAAARRR!*");
            PlaySound(0x1F8);

            IPooledEnumerable eable = Map.GetMobilesInRange(Location, 8);
            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                {
                    DoHarmful(m);
                    if (m is Mobile targetMobile)
                    {
                        int drain = Utility.RandomMinMax(20, 40);
                        if (targetMobile.Stam >= drain)
                        {
                            targetMobile.Stam -= drain;
                            targetMobile.SendMessage("You feel drained by the savage roar!");
                            targetMobile.FixedParticles(0x374A, 10, 15, 5032, EffectLayer.Head);
                        }
                    }
                }
            }
            eable.Free();
        }

        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                Say("*The mountain claims you...*");
                Effects.PlaySound(Location, Map, 0x214);
                Effects.SendLocationParticles(
                    EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                    0x376A, 10, 60, UniqueHue, 0, 5039, 0
                );

                // Scatter landmines around the corpse
                int count = Utility.RandomMinMax(3, 6);
                for (int i = 0; i < count; i++)
                {
                    int x = X + Utility.RandomMinMax(-3, 3);
                    int y = Y + Utility.RandomMinMax(-3, 3);
                    int z = Z;
                    if (!Map.CanFit(x, y, z, 16, false, false))
                        z = Map.GetAverageZ(x, y);

                    LandmineTile land = new LandmineTile();
                    land.MoveToWorld(new Point3D(x, y, z), Map);
                    Effects.SendLocationParticles(
                        EffectItem.Create(new Point3D(x, y, z), Map, EffectItem.DefaultDuration),
                        0x376A, 5, 20, UniqueHue, 0, 5039, 0
                    );
                }
            }

            base.OnDeath(c);
        }

        public override bool BleedImmune => true;
        public override int TreasureMapLevel => 7;
        public override double DispelDifficulty => 160.0;
        public override double DispelFocus => 80.0;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(10, 15));
            AddLoot(LootPack.MedScrolls, Utility.RandomMinMax(1, 2));

            if (Utility.RandomDouble() < 0.05)
                PackItem(new LabyrinthMastersHelm());

            if (Utility.RandomDouble() < 0.10)
                PackItem(new MaxxiaScroll());
        }

        public MountainMinotaur(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Reset timers
            m_NextStompTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextChargeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_NextRoarTime   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
        }
    }
}
