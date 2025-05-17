using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a matriarch of despair corpse")]
    public class MatriarchOfDespair : BaseCreature
    {
        // Ability cooldown timers
        private DateTime m_NextWailTime;
        private DateTime m_NextSpawnTime;
        private DateTime m_NextWebTime;
        private DateTime m_NextFogTime;
        private Point3D m_LastLocation;

        // A deep, unsettling crimson hue
        private const int UniqueHue = 1175;

        [Constructable]
        public MatriarchOfDespair()
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "a Matriarch of Despair";
            Body = 72;               // Terathan matriarch body
            BaseSoundID = 599;       // Terathan sounds
            Hue = UniqueHue;

            // ——— Stat Blocks ———
            SetStr(500, 600);
            SetDex(200, 250);
            SetInt(600, 700);

            SetHits(2000, 2400);
            SetStam(300, 350);
            SetMana(800, 1000);

            SetDamage(20, 30);

            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Poison, 30);
            SetDamageType(ResistanceType.Energy, 50);

            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire,     40, 50);
            SetResistance(ResistanceType.Cold,     40, 50);
            SetResistance(ResistanceType.Poison,   80, 90);
            SetResistance(ResistanceType.Energy,   70, 80);

            // ——— Skills ———
            SetSkill(SkillName.EvalInt,    115.0, 125.0);
            SetSkill(SkillName.Magery,     115.0, 125.0);
            SetSkill(SkillName.MagicResist,120.0, 130.0);
            SetSkill(SkillName.Tactics,     90.0, 100.0);
            SetSkill(SkillName.Wrestling,   90.0, 100.0);
            SetSkill(SkillName.Meditation, 100.0, 110.0);

            Fame = 25000;
            Karma = -25000;
            VirtualArmor = 90;
            ControlSlots = 5;

            // Initialize cooldowns
            var now = DateTime.UtcNow;
            m_NextWailTime  = now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextSpawnTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 30));
            m_NextWebTime   = now + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextFogTime   = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));

            m_LastLocation = this.Location;

            // Loot reagents
            PackItem(new SpidersSilk(Utility.RandomMinMax(10, 15)));
            PackItem(new BlackPearl(Utility.RandomMinMax(10, 15)));
            PackItem(new Nightshade(Utility.RandomMinMax(10, 15)));
            PackItem(new SulfurousAsh(Utility.RandomMinMax(10, 15)));
        }

        public MatriarchOfDespair(Serial serial)
            : base(serial)
        {
        }

        // Despair Aura: drains mana/stam when players move near
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (!Alive || m == this || m.Map != this.Map)
                return;

            if (m.InRange(this.Location, 2) && CanBeHarmful(m, false))
            {
                // Ensure we only affect Mobiles safely
                if (m is Mobile target && SpellHelper.ValidIndirectTarget(this, target))
                {
                    DoHarmful(target);

                    // Mana drain
                    int drain = Utility.RandomMinMax(5, 15);
                    if (target.Mana >= drain)
                    {
                        target.Mana -= drain;
                        target.SendMessage(0x22, "A wave of despair saps your magical will!");
                        target.FixedParticles(0x376A, 10, 15, 5032, UniqueHue, 0, EffectLayer.Head);
                        target.PlaySound(0x1E9);
                    }

                    // Stamina drain
                    drain = Utility.RandomMinMax(5, 15);
                    if (target.Stam >= drain)
                    {
                        target.Stam -= drain;
                    }
                }
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            if (!Alive || Combatant == null || Map == null || Map == Map.Internal)
                return;

            var now = DateTime.UtcNow;

            // Summon Despairlings
            if (now >= m_NextSpawnTime && InRange(Combatant.Location, 12))
            {
                SummonDespairlings();
                m_NextSpawnTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 40));
            }
            // Despairful Wail: fear + poison
            else if (now >= m_NextWailTime && InRange(Combatant.Location, 8))
            {
                DespairfulWail();
                m_NextWailTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
            // Cursed Web Trap
            else if (now >= m_NextWebTime)
            {
                CursedWebTrap();
                m_NextWebTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            }
            // Toxic Fog
            else if (now >= m_NextFogTime)
            {
                ToxicFogEnvelop();
                m_NextFogTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }
        }

        // ——— Ability: Despairful Wail ———
        private void DespairfulWail()
        {
            if (Combatant is Mobile target && CanBeHarmful(target, false))
            {
                Say("*A howl of agony rends your soul!*");
                PlaySound(0x2F3);

                // Fear effect
                target.FixedParticles(0x374A, 1, 15, 9502, UniqueHue, 0, EffectLayer.Waist);
                target.SendMessage("You are overcome with terror!");

                // Apply poison
                target.ApplyPoison(this, Poison.Lethal);
            }
        }

        // ——— Ability: Summon Despairlings ———
        private void SummonDespairlings()
        {
            Say("*Arise, my children of sorrow!*");
            PlaySound(0x226);

            for (int i = 0; i < 3; i++)
            {
                Point3D spawn = new Point3D(
                    X + Utility.RandomMinMax(-2, 2),
                    Y + Utility.RandomMinMax(-2, 2),
                    Z);

                if (!Map.CanFit(spawn.X, spawn.Y, spawn.Z, 16, false, false))
                    spawn.Z = Map.GetAverageZ(spawn.X, spawn.Y);

                var minion = new TerathanMatriarch(); // reusing basic matriarch as minion base
                minion.MoveToWorld(spawn, Map);
                minion.Hue = UniqueHue;

            }
        }

        // ——— Ability: Cursed Web Trap ———
        private void CursedWebTrap()
        {
            Say("*The web of despair ensnares you!*");
            PlaySound(0x2A2);

            var tile = new TrapWeb();
            tile.Hue = UniqueHue;
            tile.MoveToWorld(Combatant.Location, Map);
        }

        // ——— Ability: Toxic Fog Envelop ———
        private void ToxicFogEnvelop()
        {
            Say("*Breathe the poison of hopelessness!*");
            PlaySound(0x4F1);

            for (int i = 0; i < 6; i++)
            {
                int dx = Utility.RandomMinMax(-3, 3), dy = Utility.RandomMinMax(-3, 3);
                var loc = new Point3D(X + dx, Y + dy, Z);

                if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                var gas = new ToxicGasTile();
                gas.Hue = UniqueHue;
                gas.MoveToWorld(loc, Map);
            }
        }

        // ——— Death Effect: Final Despair ———
        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                Say("*My sorrow... consumes all...*");
                Effects.PlaySound(Location, Map, 0x214);
                Effects.SendLocationParticles(
                    EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                    0x376A, 20, 60, UniqueHue, 0, 5032, 0);

                // Spawn lingering poison mist
                for (int i = 0; i < 8; i++)
                {
                    int dx = Utility.RandomMinMax(-4, 4), dy = Utility.RandomMinMax(-4, 4);
                    var loc = new Point3D(X + dx, Y + dy, Z);

                    if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                        loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                    var mist = new PoisonTile();
                    mist.Hue = UniqueHue;
                    mist.MoveToWorld(loc, Map);
                }
            }

            base.OnDeath(c);
        }

        // ——— Loot & Properties ———
        public override bool BleedImmune => true;
        public override int TreasureMapLevel => 6;
        public override double DispelDifficulty => 150.0;
        public override double DispelFocus    => 75.0;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.MedScrolls, 3);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(10, 15));

            // 5% chance for unique despair artifact
            if (Utility.RandomDouble() < 0.05)
            {
                PackItem(new MaxxiaScroll()); // placeholder for unique item
            }
        }

        // Serialization
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
            var now = DateTime.UtcNow;
            m_NextWailTime  = now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextSpawnTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 30));
            m_NextWebTime   = now + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextFogTime   = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));

            m_LastLocation = this.Location;
        }
    }
}
