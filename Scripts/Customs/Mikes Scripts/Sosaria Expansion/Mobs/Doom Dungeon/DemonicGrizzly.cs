using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a demonic grizzly corpse")]
    public class DemonicGrizzly : BaseCreature
    {
        // Cooldown timers for special abilities
        private DateTime m_NextRoarTime;
        private DateTime m_NextChargeTime;
        private DateTime m_NextHellfireTime;

        // Track last location for footprints
        private Point3D m_LastLocation;

        // A blood‑red hue
        private const int UniqueHue = 1645;

        [Constructable]
        public DemonicGrizzly()
            : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "a demonic grizzly";
            Body = 212;
            BaseSoundID = 0xA3;
            Hue = UniqueHue;

            // ——— Stats ———
            SetStr(1550, 1750);
            SetDex(300, 400);
            SetInt(200, 300);

            SetHits(1200, 1500);
            SetMana(0);

            SetDamage(25, 35);

            SetDamageType(ResistanceType.Physical, 70);
            SetDamageType(ResistanceType.Fire, 30);

            SetResistance(ResistanceType.Physical, 60, 80);
            SetResistance(ResistanceType.Fire, 90, 100);
            SetResistance(ResistanceType.Cold, 30, 50);
            SetResistance(ResistanceType.Poison, 50, 70);
            SetResistance(ResistanceType.Energy, 40, 60);

            SetSkill(SkillName.Wrestling, 100.1, 120.0);
            SetSkill(SkillName.Tactics,   100.1, 120.0);
            SetSkill(SkillName.MagicResist,100.1, 120.0);

            Fame = 25000;
            Karma = -25000;
            VirtualArmor = 90;
            ControlSlots = 6;
            Tamable = false;

            // Initialize timers
            m_NextRoarTime     = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
            m_NextChargeTime   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_NextHellfireTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            m_LastLocation     = this.Location;

            // Basic loot setup
            PackGold(500, 800);
            PackItem(new SulfurousAsh(Utility.RandomMinMax(5, 10)));
        }

        // Leave hellish lava footprints when others move nearby
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (m != null && m != this && m.Map == Map && m.InRange(Location, 2) && Alive)
            {
                if (Utility.RandomDouble() < 0.15)
                {
                    var loc = oldLocation;
                    if (Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    {
                        var lava = new HotLavaTile { Hue = UniqueHue };
                        lava.MoveToWorld(loc, Map);
                    }
                }
            }
            base.OnMovement(m, oldLocation);
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || Map == null || Map == Map.Internal || !Alive)
            {
                m_LastLocation = Location;
                return;
            }

            var now = DateTime.UtcNow;

            // Hellish Roar: area stun
            if (now >= m_NextRoarTime && InRange(Combatant.Location, 8))
            {
                HellishRoar();
                m_NextRoarTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 45));
            }
            // Demonic Charge: leap & slam
            else if (now >= m_NextChargeTime && InRange(Combatant.Location, 12))
            {
                DemonicCharge();
                m_NextChargeTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
            // Summon Hellfire Pit: ground hazard under the player
            else if (now >= m_NextHellfireTime)
            {
                SummonHellfirePit();
                m_NextHellfireTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
            }

            m_LastLocation = Location;
        }

        private void HellishRoar()
        {
            Say("*a hellish roar echoes!*");
            PlaySound(0x64A);
            FixedParticles(0x373A, 1, 30, 9502, UniqueHue, 0, EffectLayer.Head);

            var eable = Map.GetMobilesInRange(Location, 8);
            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m) && m is Mobile target)
                {
                    DoHarmful(target);
                    target.Paralyze(TimeSpan.FromSeconds(3));
                    target.SendMessage("You are shaken by the demonic roar!");
                }
            }
            eable.Free();
        }

        private void DemonicCharge()
        {
            if (Combatant is Mobile target && CanBeHarmful(target, false))
            {
                Say("*charges with demonic fury!*");
                PlaySound(0x56D);

                // Teleport into melee range if needed
                if (!InRange(target.Location, 2))
                {
                    Location = target.Location;
                    PlaySound(0x55F);
                }

                DoHarmful(target);
                AOS.Damage(target, this, Utility.RandomMinMax(60, 80), 0, 0, 0, 0, 100);
            }
        }

        private void SummonHellfirePit()
        {
            if (Combatant is Mobile target && CanBeHarmful(target, false))
            {
                Say("*flames erupt beneath you!*");
                PlaySound(0x208);

                var loc = target.Location;
                Timer.DelayCall(TimeSpan.FromSeconds(0.5), () =>
                {
                    if (Map == null) return;

                    var spawn = loc;
                    if (!Map.CanFit(spawn.X, spawn.Y, spawn.Z, 16, false, false))
                    {
                        spawn.Z = Map.GetAverageZ(spawn.X, spawn.Y);
                        if (!Map.CanFit(spawn.X, spawn.Y, spawn.Z, 16, false, false))
                            return;
                    }

                    var pit = new FlamestrikeHazardTile { Hue = UniqueHue };
                    pit.MoveToWorld(spawn, Map);
                });
            }
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            if (Map == null) return;

            PlaySound(0x211);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x372A, 15, 20, UniqueHue, 0, 5037, 0);

            for (int i = 0; i < 6; i++)
            {
                int x = Utility.RandomMinMax(-3, 3);
                int y = Utility.RandomMinMax(-3, 3);
                var loc = new Point3D(X + x, Y + y, Z);

                if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                var pit = new FlamestrikeHazardTile { Hue = UniqueHue };
                pit.MoveToWorld(loc, Map);
            }
        }

        // Standard overrides
        public override bool BleedImmune => true;
        public override Poison PoisonImmune => Poison.Lethal;
        public override int TreasureMapLevel => 6;
        public override double DispelDifficulty => 150.0;
        public override double DispelFocus    =>  80.0;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 3);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(10, 15));

            // 5% chance to drop a unique demonic claw
            if (Utility.RandomDouble() < 0.05)
                PackItem(new ThistlelineGreaves());
        }

        // Serialization
        public DemonicGrizzly(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
